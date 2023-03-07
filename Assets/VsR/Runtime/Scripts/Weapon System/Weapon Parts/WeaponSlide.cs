using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

namespace VsR {
	public class WeaponSlide : XRBaseInteractable {
		[HideInInspector] public WeaponBase weapon;
		[SerializeField] protected float m_maxSlideOffset;
		[SerializeField] protected float m_releaseAnimationSpeed = 0.002f;

		public delegate void LockEvent();
		public event LockEvent onLocked;
		public event LockEvent onUnlocked;

		protected Vector3 m_initPosition;
		protected Vector3 m_maxSlidePosition;
		protected Vector3 m_startHandLocalPosition;
		protected bool m_racked = false;
		protected Hand m_hand;

		private bool _m_locked = false;
		public bool Locked {
			get => _m_locked;
			set {
				_m_locked = value;
				if (_m_locked)
					onLocked?.Invoke();
				else
					onUnlocked?.Invoke();
			}
		}

		public bool Racked => m_racked;
		protected Vector3 getHandLocalPosition() => transform.InverseTransformPoint(m_hand.attachTransform.position);

		protected override void Awake() {
			base.Awake();
			m_initPosition = transform.localPosition;
		}

		protected virtual void Start() {
			m_maxSlidePosition = m_initPosition - transform.InverseTransformDirection(weapon.transform.forward) * m_maxSlideOffset;
		}

		protected virtual void UpdateSlideMovement() {
			if (Locked)
				return;

			transform.localPosition = m_initPosition;
			float slide = weapon.transform.InverseTransformDirection(transform.TransformDirection(m_startHandLocalPosition - getHandLocalPosition())).z;
			float slidePercentage = Mathf.Clamp01(slide / m_maxSlideOffset);

			transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, slidePercentage);

			if (m_racked && slidePercentage < 0.6f)
				RackBack();
			else if (!m_racked && slidePercentage >= 0.99f)
				Rack();
		}

		protected void Rack() {
			SoundPoolManager.Instance.PlaySound(weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));
			weapon.TryToCock();
			m_racked = true;
		}

		protected void RackBack() {
			SoundPoolManager.Instance.PlaySound(weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_racked = false;
		}

		protected IEnumerator ReleaseAnimation() {
			while (Vector3.Distance(transform.localPosition, m_initPosition) != 0) {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_initPosition, m_releaseAnimationSpeed * Time.deltaTime);
				yield return null;
			}
		}

		public void OnRelease() {
			if (m_racked)
				RackBack();

			StartCoroutine(ReleaseAnimation());
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (!weapon.isSelected)
				return false;

			if (Locked)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (Hand)args.interactorObject;

			m_startHandLocalPosition = getHandLocalPosition();
		}

		protected override void OnSelectExiting(SelectExitEventArgs args) {
			base.OnSelectExiting(args);

			if (!Locked)
				OnRelease();

			m_hand = null;
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (isSelected && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
				UpdateSlideMovement();
		}
	}
}
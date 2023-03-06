using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponSlide : XRBaseInteractable {
		[HideInInspector] public WeaponBase weapon;
		[SerializeField] protected float m_maxSlideOffset;

		protected Vector3 m_initPosition;
		protected Vector3 m_maxSlidePosition;

		protected Vector3 m_startHandLocalPosition;

		protected bool m_racked = false;
		protected Hand m_hand;

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
			transform.localPosition = m_initPosition;

			float slide = weapon.transform.InverseTransformDirection(transform.TransformDirection(m_startHandLocalPosition - getHandLocalPosition())).z;
			float slidePercentage = Mathf.Clamp01(slide / m_maxSlideOffset);

			transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, slidePercentage);

			if (m_racked && slidePercentage < 0.6f)
				RackBack();
			else if (!m_racked && slidePercentage >= 0.99f)
				Rack();
		}

		protected virtual void Rack() {
			SoundPoolManager.Instance.PlaySound(weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));
			weapon.TryToCock();
			m_racked = true;
		}

		protected virtual void RackBack() {
			SoundPoolManager.Instance.PlaySound(weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_racked = false;
		}

		protected virtual void OnRelease() {
			transform.localPosition = m_initPosition;
			if (m_racked)
				RackBack();
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (!weapon.isSelected)
				return false;

			if (weapon is Pistol pistol && pistol.SlideStop)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (Hand)args.interactorObject;

			m_startHandLocalPosition = getHandLocalPosition();
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);

			m_hand = null;
			OnRelease();
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (isSelected && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Late)
				UpdateSlideMovement();
		}
	}
}
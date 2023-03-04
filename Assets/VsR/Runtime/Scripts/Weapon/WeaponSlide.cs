using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VsR.Math;

namespace VsR {
	public class WeaponSlide : XRBaseInteractable {
		[HideInInspector] public Weapon weapon;
		[SerializeField] private float m_maxSlide;

		private Vector3 m_initPosition;
		private Vector3 m_maxSlidePostiion;

		private Vector3 m_startHandLocalPosition;

		private bool m_racked = false;
		private Hand m_hand;

		private Vector3 getHandLocalPosition() => transform.InverseTransformPoint(m_hand.attachTransform.position);

		protected override void Awake() {
			base.Awake();
			m_initPosition = transform.localPosition;
		}

		private void Start() {
			m_maxSlidePostiion = m_initPosition - transform.InverseTransformDirection(weapon.transform.forward) * m_maxSlide;
		}

		private void UpdateSlideMovement() {
			// TODO: There has to be a less fucked up way of doing this....
			float slide = weapon.transform.InverseTransformDirection(transform.TransformDirection(m_startHandLocalPosition - getHandLocalPosition())).z;
			float slidePercentage = Mathf.Clamp01(slide / m_maxSlide);

			transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePostiion, slidePercentage);

			if (m_racked && slidePercentage < 0.6f)
				RackBack();
			else if (!m_racked && slidePercentage >= 0.99f)
				Rack();
		}

		private void Rack() {
			SoundManager.Instance.PlaySound(weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));
			weapon.TryToCock();
			m_racked = true;
		}

		private void RackBack() {
			SoundManager.Instance.PlaySound(weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_racked = false;
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

			transform.localPosition = m_initPosition;

			if (m_racked) {
				SoundManager.Instance.PlaySound(weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));

				if (weapon is Pistol pistol)
					pistol.SlideStop = false;
			}

			m_racked = false;
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (m_hand != null && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Late)
				UpdateSlideMovement();
		}
	}
}
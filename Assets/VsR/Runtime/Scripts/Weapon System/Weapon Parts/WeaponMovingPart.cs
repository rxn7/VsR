using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VsR {
	public class WeaponMovingPart : XRBaseInteractable, IWeaponPart {
		public enum SlideAxis { X, Y, Z, NEG_X, NEG_Y, NEG_Z }

		public event System.Action onRelease;

		[field: SerializeField] public Weapon Weapon { get; set; }

		[SerializeField] protected Vector3 m_maxSlidePosition;
		[SerializeField] protected SlideAxis m_slideAxis = SlideAxis.Z;
		[SerializeField] protected bool m_canInteractWithoutWeaponSelected = false;
		protected Vector3 m_initPosition;
		protected float m_initToMaxSlideDistance;
		protected Vector3 m_startHandLocalPosition;
		protected Hand m_hand;

		protected Vector3 getHandLocalPosition() => transform.InverseTransformPoint(m_hand.attachTransform.position);
		protected virtual bool CanRelease => true;

		protected override void Awake() {
			base.Awake();
			m_initPosition = transform.localPosition;
			m_initToMaxSlideDistance = Vector3.Distance(m_initPosition, m_maxSlidePosition);
			interactionLayers = InteractionLayerMask.GetMask("Hand");
		}

		public override bool IsSelectableBy(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor) {
			if (!m_canInteractWithoutWeaponSelected && !Weapon.GripHand)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected virtual float UpdateSlideMovement() {
			transform.localPosition = m_initPosition;

			int axisIdx = (int)m_slideAxis % 3;
			float handDifference = Weapon.transform.InverseTransformDirection(transform.TransformDirection(m_startHandLocalPosition - getHandLocalPosition()))[axisIdx];
			if ((int)m_slideAxis > 2)
				handDifference *= -1;

			float slidePercentage = Mathf.Clamp01(handDifference / m_initToMaxSlideDistance);

			transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, slidePercentage);

			return slidePercentage;
		}

		public virtual void Release() {
			onRelease?.Invoke();
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (Hand)args.interactorObject;

			m_startHandLocalPosition = getHandLocalPosition();
		}

		protected override void OnSelectExiting(SelectExitEventArgs args) {
			base.OnSelectExiting(args);
			m_hand = null;

			if (CanRelease)
				Release();
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (isSelected && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
				UpdateSlideMovement();
		}
	}
}
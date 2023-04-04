using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponMovingPart : XRBaseInteractable, IWeaponPart {
		public event System.Action onRelease;

		[field: SerializeField] public Weapon Weapon { get; set; }

		[SerializeField] protected Vector3 m_maxSlidePosition;
		[SerializeField] protected Math.Axis m_slideAxis = Math.Axis.Z;
		[SerializeField] protected bool m_axisNegative = false;
		[SerializeField] protected bool m_canInteractWithoutWeaponSelected = false;
		protected float m_maxSlideValue;
		protected Vector3 m_initPosition;
		protected Vector3 m_startHandLocalPosition;
		protected HandInteractor m_hand;

		protected Vector3 getHandLocalPosition() => transform.InverseTransformPoint(m_hand.attachTransform.position);
		protected virtual bool CanRelease => true;

		protected override void Awake() {
			base.Awake();
			IWeaponPart.Validate(this);

			m_initPosition = transform.localPosition;
			m_maxSlideValue = Vector3.Distance(m_initPosition, m_maxSlidePosition);
			interactionLayers = InteractionLayerMask.GetMask("Hand");
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (!m_canInteractWithoutWeaponSelected && !Weapon.GripHand)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected virtual float UpdateSlideMovement() {
			transform.localPosition = m_initPosition;

			int axisIdx = (int)m_slideAxis;
			float handDifference = Weapon.transform.InverseTransformDirection(transform.TransformDirection(m_startHandLocalPosition - getHandLocalPosition()))[axisIdx];
			if (m_axisNegative)
				handDifference *= -1;

			float slidePercentage = Mathf.Clamp01(handDifference / m_maxSlideValue);

			transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, slidePercentage);

			return slidePercentage;
		}

		public virtual void Release() {
			onRelease?.Invoke();
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (HandInteractor)args.interactorObject;

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
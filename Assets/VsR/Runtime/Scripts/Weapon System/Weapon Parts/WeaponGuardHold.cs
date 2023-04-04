using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponGuardHold : XRBaseInteractable, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		protected HandInteractor m_hand;
		protected Quaternion m_initGripRotation;

		protected override void Awake() {
			base.Awake();
			IWeaponPart.Validate(this);
			interactionLayers = InteractionLayerMask.GetMask("Hand");
		}

		private void ProcessWeaponMovement() {
			if (m_hand == null || !Weapon.GripHand)
				return;

			Weapon.GripHand.attachTransform.rotation = Quaternion.LookRotation(m_hand.attachTransform.position - Weapon.GripHand.attachTransform.position, Weapon.GripHand.transform.up);
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (!Weapon.GripHand)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (HandInteractor)args.interactorObject;
			m_initGripRotation = Weapon.GripHand.attachTransform.localRotation;
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);
			m_hand = null;
			Weapon.GripHand.attachTransform.localRotation = m_initGripRotation;
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Late)
				ProcessWeaponMovement();
		}
	}
}

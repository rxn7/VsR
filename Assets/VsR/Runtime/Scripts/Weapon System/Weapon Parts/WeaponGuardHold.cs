using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponGuardHold : XRBaseInteractable, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		protected Hand m_hand;
		protected Quaternion m_initGripRotation;

		private void ProcessWeaponMovement() {
			if (m_hand == null || !Weapon.isSelected)
				return;

			Hand gripHand = Weapon.GripHand;
			gripHand.attachTransform.rotation = GetGripRotation();
		}

		private Quaternion GetGripRotation() {
			return Quaternion.LookRotation(m_hand.attachTransform.position - Weapon.GripHand.attachTransform.position, Weapon.GripHand.attachTransform.up);
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand || !Weapon.isSelected)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (Hand)args.interactorObject;
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

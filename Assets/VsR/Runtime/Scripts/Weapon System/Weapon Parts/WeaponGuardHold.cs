using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponGuardHold : XRBaseInteractable, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		protected XRBaseControllerInteractor m_interactor;
		protected Quaternion m_initGripRotation;

		protected override void Awake() {
			base.Awake();
			IWeaponPart.Validate(this);
			interactionLayers = InteractionLayerMask.GetMask("Hand");
		}

		private void ProcessWeaponMovement() {
			if (!m_interactor || !Weapon.GripHand)
				return;

			Weapon.GripHand.Interactor.attachTransform.rotation = Quaternion.LookRotation(m_interactor.attachTransform.position - Weapon.GripHand.Interactor.attachTransform.position, Weapon.GripHand.transform.up);
		}

		public override bool IsHoverableBy(IXRHoverInteractor interactor) {
			if (!Weapon.GripHand)
				return false;

			return base.IsHoverableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_interactor = args.interactorObject as XRBaseControllerInteractor;
			m_initGripRotation = Weapon.GripHand.Interactor.attachTransform.localRotation;
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);
			m_interactor = null;
			Weapon.GripHand.Interactor.attachTransform.localRotation = m_initGripRotation;
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Late)
				ProcessWeaponMovement();
		}
	}
}

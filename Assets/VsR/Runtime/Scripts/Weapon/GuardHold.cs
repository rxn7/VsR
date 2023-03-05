using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class GuardHold : XRBaseInteractable {
		[HideInInspector] public DoubleHandedWeapon weapon;

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (!weapon.isSelected)
				return false;

			return base.IsSelectableBy(interactor);
		}
	}
}

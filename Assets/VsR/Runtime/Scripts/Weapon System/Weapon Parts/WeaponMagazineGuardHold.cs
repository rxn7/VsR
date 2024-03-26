using UnityEngine;


namespace VsR {
	public class WeaponMagazineGuardHold : WeaponGuardHold {
		[SerializeField] private WeaponMagazineSlot m_magSlot;

		public override bool IsSelectableBy(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor) {
			if (!m_magSlot.Mag)
				return false;

			return base.IsSelectableBy(interactor);
		}
	}
}

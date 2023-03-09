using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponMagazineGuardHold : WeaponGuardHold {
		[SerializeField] private WeaponMagazineSlot m_magSlot;

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (!m_magSlot.Mag)
				return false;

			if (interactor is not Hand || !Weapon.isSelected)
				return false;

			return base.IsSelectableBy(interactor);
		}
	}
}
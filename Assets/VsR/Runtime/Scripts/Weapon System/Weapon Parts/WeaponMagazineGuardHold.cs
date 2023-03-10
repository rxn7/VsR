using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponMagazineGuardHold : WeaponGuardHold {
		[SerializeField] private WeaponMagazineSlot m_magSlot;

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (!m_magSlot.Mag)
				return false;

			return base.IsSelectableBy(interactor);
		}
	}
}

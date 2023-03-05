using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class DoubleHandedWeapon : WeaponBase {
		[SerializeField] private GuardHold m_guardHold;

		protected override void Awake() {
			base.Awake();

			m_guardHold.weapon = this;
		}

		private void ProcessDoubleGrip() {
			// TODO; 
		}
	}
}
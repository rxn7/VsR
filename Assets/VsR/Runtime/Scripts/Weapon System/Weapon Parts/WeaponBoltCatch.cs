using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

namespace VsR {
	public class WeaponBoltCatch : WeaponPart {
		[SerializeField] private WeaponBolt m_bolt;
		[SerializeField] private MagazineSlot m_magSlot;

		protected override void Awake() {
			base.Awake();
			m_weapon.onFire += OnFire;
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			interactionManager.SelectExit(args.interactorObject, this);

			if (!m_bolt.Open) return;

			m_bolt.Open = false;
			m_weapon.TryToCock();
		}

		private void OnFire() {
			if (m_magSlot.Mag && m_magSlot.Mag.IsEmpty) {
				m_bolt.Open = true;
			}
		}
	}
}
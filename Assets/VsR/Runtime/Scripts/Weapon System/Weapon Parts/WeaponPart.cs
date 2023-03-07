using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponPart : XRBaseInteractable {
		[SerializeField] protected WeaponBase m_weapon;
	}
}
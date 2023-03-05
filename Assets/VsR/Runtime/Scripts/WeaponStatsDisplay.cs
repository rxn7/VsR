using UnityEngine;

namespace VsR {
	[ExecuteInEditMode]
	[RequireComponent(typeof(TMPro.TMP_Text))]
	public class WeaponStatsDisplay : MonoBehaviour {
		[SerializeField] WeaponData m_data;

		private void OnEnable() {
			if (m_data == null)
				return;

			TMPro.TMP_Text textMesh = GetComponent<TMPro.TMP_Text>();
			textMesh.text = $"{m_data.displayName}\n" +
							$"Muzzle vel: {m_data.muzzleVelocity}\n" +
							$"Rounds/min: {(m_data.shootType != WeaponData.ShootType.Automatic ? "∞" : m_data.roundsPerMinute)}";
		}
	}
}
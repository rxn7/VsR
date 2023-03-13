using UnityEngine;
using System.Text;

namespace VsR {
	[ExecuteInEditMode]
	[RequireComponent(typeof(TMPro.TMP_Text))]
	public class WeaponStatsDisplay : MonoBehaviour {
		[SerializeField] WeaponData m_data;

		private void Awake() {
		}

		private void OnEnable() {
			if (!m_data)
				return;

			StringBuilder str = new StringBuilder(200);
			str.AppendLine(m_data.name);
			str.AppendLine($"Muzzle vel: {m_data.muzzleVelocity}");
			str.AppendLine($"Rounds/min: {m_data.roundsPerMinute}");
			str.AppendLine($"Cartridge: {m_data.cartridgeData.name}");

			GetComponent<TMPro.TMP_Text>().text = str.ToString();
		}
	}
}
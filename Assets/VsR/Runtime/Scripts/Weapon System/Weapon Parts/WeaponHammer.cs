using UnityEngine;

namespace VsR {
	public class WeaponHammer : MonoBehaviour {
		[SerializeField] private Math.FloatRange m_range;

		public void UpdateRotation(float normalizedTriggerValue, WeaponData data, bool triggerReset) {
			float value = Mathf.Clamp01(normalizedTriggerValue / data.fireTriggerValue);
			transform.localEulerAngles = new Vector3(Mathf.Lerp(m_range.min, m_range.max, value), 0, 0);
		}
	}
}
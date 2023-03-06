using UnityEngine;

namespace VsR {
	public class WeaponTrigger : MonoBehaviour {
		[SerializeField] private Math.FloatRange m_range;
		public void UpdateRotation(float normalizedTriggerValue) {
			transform.localEulerAngles = new Vector3(Mathf.Lerp(m_range.min, m_range.max, normalizedTriggerValue), 0, 0);
		}
	}
}
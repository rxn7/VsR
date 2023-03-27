using UnityEngine;

namespace VsR {
	public class WeaponTrigger : MonoBehaviour {
		public enum TriggerType {
			Rotation,
			Position
		};

		[SerializeField] private TriggerType m_type;
		[SerializeField] private Math.Axis m_axis;
		[SerializeField] private float m_maxValue;
		private float m_initValue;

		private void Awake() {
			switch (m_type) {
				case TriggerType.Rotation:
					m_initValue = transform.localEulerAngles[(int)m_axis];
					break;

				case TriggerType.Position:
					m_initValue = transform.localPosition[(int)m_axis];
					break;
			}
		}

		public void UpdateRotation(float normalizedTriggerValue) {
			switch (m_type) {
				case TriggerType.Rotation:
					Vector3 eulerAngles = transform.localEulerAngles;
					eulerAngles[(int)m_axis] = Mathf.Lerp(m_initValue, m_maxValue, normalizedTriggerValue);
					transform.localEulerAngles = eulerAngles;
					break;

				case TriggerType.Position:
					Vector3 pos = transform.localPosition;
					pos[(int)m_axis] = Mathf.Lerp(m_initValue, m_maxValue, normalizedTriggerValue);
					transform.localPosition = pos;
					break;
			}
		}
	}
}
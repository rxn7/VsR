using UnityEngine;

namespace VsR {
	public class ToggleSlowMotion : MonoBehaviour {
		[SerializeField] private float m_slowMotionTimeScale = 0.2f;
		[SerializeField] private float m_normalTimeScale = 1.0f;

		public void Toggle(bool slowMotion) {
			TimeManager.TimeScale = slowMotion ? m_slowMotionTimeScale : m_normalTimeScale;
		}
	}
}
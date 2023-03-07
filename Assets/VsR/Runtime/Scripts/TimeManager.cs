using UnityEngine;

namespace VsR {
	public class TimeManager : MonoBehaviour {
		[SerializeField] private float m_timeScale = 1.0f;

		public void FixedUpdate() {
			Time.timeScale = m_timeScale;
		}
	}
}

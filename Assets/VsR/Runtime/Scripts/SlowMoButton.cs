using UnityEngine;

namespace VsR {
	[RequireComponent(typeof(Button))]
	public class SlowMoButton : MonoBehaviour {
		private bool m_slowMo = false;

		private void Awake() {
			GetComponent<Button>().onPressed += Pressed;
		}

		private void Pressed() {
			if (m_slowMo) {
				TimeManager.TimeScale = 1.0f;
				m_slowMo = false;
			} else {
				TimeManager.TimeScale = 0.05f;
				m_slowMo = true;
			}
		}
	}
}

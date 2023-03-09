using UnityEngine;

namespace VsR {
	public static class TimeManager {
		public delegate void TimeScaleEvent(float timeScale);
		public static event TimeScaleEvent onTimeScaleChanged;

		public static float TimeScale {
			get => Time.timeScale;
			set {
				Time.timeScale = value;
				onTimeScaleChanged?.Invoke(value);
			}
		}
	}
}

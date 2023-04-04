using UnityEngine;

namespace VsR.Math {
	[System.Serializable]
	public struct FloatRange {
		public float min;
		public float max;

		public FloatRange(float min, float max) {
			this.min = min;
			this.max = max;
		}

		public float Percentage(float v) => Mathf.Clamp01((v - min) / (max - min));
		public float Clamp(float v) => Mathf.Clamp(v, min, max);

		public float RandomValue() => Random.Range(min, max);
	}
}
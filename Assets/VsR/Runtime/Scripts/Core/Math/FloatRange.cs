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

		public float RandomValue() => Random.Range(min, max);
	}
}
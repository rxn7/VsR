using UnityEngine;

namespace VsR.Math {
	[System.Serializable]
	public struct Vector3Range {
		public FloatRange x, y, z;

		public Vector3Range(FloatRange x, FloatRange y, FloatRange z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3 RandomValue() => new Vector3(x.RandomValue(), y.RandomValue(), z.RandomValue());
	}
}
using UnityEngine;

namespace VsR.Math {
	public static class VectorHelper {
		public static Vector3 RandomVector(FloatRange x, FloatRange y, FloatRange z) {
			return new Vector3(Random.Range(x.min, x.max), Random.Range(y.min, y.max), Random.Range(z.min, z.max));
		}
	}
}
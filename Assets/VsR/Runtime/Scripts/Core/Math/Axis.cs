using UnityEngine;

namespace VsR.Math {
	public enum Axis { X, Y, Z }

	public static class AxisHelper {
		public static Vector3 DirectionFromAxis(Axis axis) {
			Vector3 v = new Vector3();
			v[(int)axis] = 1.0f;
			return v;
		}
	}
}
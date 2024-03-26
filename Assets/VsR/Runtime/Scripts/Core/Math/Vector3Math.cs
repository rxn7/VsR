using UnityEngine;

namespace VsR.Math {
    public static class Vector3Math {
        public static float InverseLerp(Vector3 v, Vector3 a, Vector3 b) {
            Vector3 ab = b - a;
            Vector3 av = v - a;
            return Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
        }
    }
}
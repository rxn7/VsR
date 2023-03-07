using UnityEngine;

namespace VsR {
	[System.Serializable]
	public struct RecoilInfo {
		public Vector3 force;
		public Vector3 forceRandomness;
		public Vector3 torque;
		public Vector3 torqueRandomness;
	}
}
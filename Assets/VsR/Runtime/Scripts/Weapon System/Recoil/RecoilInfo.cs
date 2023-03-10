using UnityEngine;
using VsR.Math;

namespace VsR {
	[System.Serializable]
	public struct RecoilInfo {
		public Vector3Range force;
		public Vector3Range torque;
	}
}
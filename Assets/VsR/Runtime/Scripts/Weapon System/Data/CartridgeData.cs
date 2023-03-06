using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/BulletData", fileName = "bullet")]
	public class CartridgeData : ScriptableObject {
		[Header("General")]
		public float bulletMassGrams = 7;
		public float bulletDiameterMm = 9;
		public float bulletLengthMm = 19;

		[Header("References")]
		public Mesh bulletMesh;
		// public Mesh cartridgeMesh;
		public Cartridge cartridgePrefab;
	}
}
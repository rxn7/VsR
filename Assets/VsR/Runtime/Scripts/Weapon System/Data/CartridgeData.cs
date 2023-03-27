using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/BulletData", fileName = "bullet")]
	public class CartridgeData : ScriptableObject {
		[Header("General")]
		public CreditsData creditsData;
		public float bulletMassGrams = 7;
		public float bulletLengthMm = 10;
		public float bulletDiameterMm = 9;
		public float cartridgeLengthMm = 19;
		public float cartridgeRadiusMm = 10;

		[Header("References")]
		public AudioClip[] cartridgeCollisionSounds;
		public Mesh bulletMesh;
		public Mesh cartridgeMesh;

		public AudioClip GetRandomCollideSound() => cartridgeCollisionSounds.Length != 0 ? cartridgeCollisionSounds[Random.Range(0, cartridgeCollisionSounds.Length)] : null;
		public float BulletMassKg => bulletMassGrams * 0.001f;
	}
}
using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/BulletData", fileName = "bullet")]
	public class CartridgeData : ScriptableObject {
		[Header("General")]
		public float bulletMassGrams = 7;
		public float bulletDiameterMm = 9;
		public float bulletLengthMm = 19;

		[Header("References")]
		public AudioClip[] cartridgeCollideSounds;
		public Mesh bulletMesh;
		public Mesh cartridgeMesh;

		public AudioClip GetRandomCollideSound() => cartridgeCollideSounds.Length != 0 ? cartridgeCollideSounds[Random.Range(0, cartridgeCollideSounds.Length)] : null;
		public float BulletMassKg => bulletMassGrams * 0.001f;
        
        public Cartridge Spawn(Transform parent, bool withBullet) {
            Cartridge cartridge = Instantiate(Cartridge.PREFAB, parent, false).GetComponent<Cartridge>();
            cartridge.SetData(this, withBullet);
            return cartridge;
        }
	}
}

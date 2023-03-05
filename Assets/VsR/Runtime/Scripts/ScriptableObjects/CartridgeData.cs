using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/BulletData", fileName = "bullet")]
	public class CartridgeData : ScriptableObject {
		[Header("General")]
		public float caliberMm = 9;

		[Header("References")]
		public Bullet bulletPrefab;
		public Cartridge cartridgePrefab;
	}
}
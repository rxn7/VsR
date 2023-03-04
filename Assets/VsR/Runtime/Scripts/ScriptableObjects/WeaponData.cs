using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/WeaponData", fileName = "weapon")]
	public class WeaponData : ScriptableObject {
		public enum WeaponType : byte {
			Pistol,
			Rifle,
		}

		public enum ShootType : byte {
			Automatic,
			SemiAutomatic,
			Manual,
		}

		public enum ShootingPhysicsType : byte {
			Projectile,
			RaycastPhysicalBased,
			RaycastLaser,
		}

		[Header("General")]
		public string displayName;
		public ShootType shootType;
		public WeaponType weaponType;
		public ShootingPhysicsType shootingPhysicsType;
		[Range(0.0f, 1.0f)] public float fireTriggerValue = 0.3f;
		[Range(0.0f, 1.0f)] public float resetTriggerValue = 0.28f;
		public HapticFeedback fireHapticFeedback;

		[Header("Statistics")]
		public ushort roundsPerMinute = 1;
		[Tooltip("Muzzle velocity in m/s")] public float muzzleVelocity = 600;

		[Header("Visual")]
		public float minTriggerRotation;
		public float maxTriggerRotation;

		[Header("Audio")]
		public AudioClip shootSound;
		public AudioClip cockSound;
		public AudioClip cockBackSound;
		public AudioClip dryFireSound;

		[Header("Referenes")]
		public Weapon prefab;
		public CartridgeData cartridgeData;
		public MagazineData[] compatibleMagazines;

		public float SecondsPerRound => 60.0f / roundsPerMinute;
	}
}
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

		public enum BulletPhysics : byte {
			RAYCAST_PHYSICAL_BASED,
			RAYCAST_LASER,
			PROJECTILE,
		}

		[Header("General")]
		public string displayName;
		public ShootType shootType;
		public WeaponType weaponType;
		public BulletPhysics bulletPhysics;
		[Range(0.0f, 1.0f)] public float fireTriggerValue = 0.3f;
		[Range(0.0f, 1.0f)] public float resetTriggerValue = 0.28f;

		[Space]
		[Header("Statistics")]
		public ushort roundsPerMinute = 1;
		public float SecondsPerRound => 60.0f / roundsPerMinute;
		public float power = 100;

		[Space]
		[Header("Haptic Feedback")]
		[Range(0.0f, 1.0f)] public float shootHapticFeedbackIntensity = 0.3f;
		[Range(0.0f, 3.0f)] public float shootHapticFeedbackDuration = 0.2f;

		[Space]
		[Header("Visual")]
		public float minTriggerRotation;
		public float maxTriggerRotation;

		[Space]
		[Header("Audio")]
		public AudioClip shootSound;
		public AudioClip cockSound;
		public AudioClip cockBackSound;
		public AudioClip dryFireSound;

		[Space]
		[Header("Referenes")]
		public Weapon prefab;
		public CartridgeData cartridgeData;
		public MagazineData[] compatibleMagazines;
	}
}
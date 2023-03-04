using UnityEngine;
using VsR.Math;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/WeaponData", fileName = "weapon")]
	public class WeaponData : ScriptableObject {
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
		public ShootingPhysicsType shootingPhysicsType;
		public ushort roundsPerMinute = 100;
		[Tooltip("Muzzle velocity in m/s")] public float muzzleVelocity = 600;
		[Range(0.0f, 1.0f)] public float fireTriggerValue = 0.3f;
		[Range(0.0f, 1.0f)] public float resetTriggerValue = 0.28f;

		[Header("Feedback")]
		public FloatRange triggerRotationRange;
		public HapticFeedback fireHapticFeedback;

		[Header("Audio")]
		public AudioClip shootSound;
		public AudioClip cockSound;
		public AudioClip cockBackSound;
		public AudioClip dryFireSound;

		[Header("Referenes")]
		public GameObject prefab;
		public CartridgeData cartridgeData;
		public MagazineData[] compatibleMagazines;

		public float SecondsPerRound => 60.0f / roundsPerMinute;
	}
}
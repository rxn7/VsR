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
		public ShootType shootType;
		public ShootingPhysicsType shootingPhysicsType;

		[Tooltip("This will overwrite rigidbody's mass")]
		public float massKg = 1.0f;

		[Tooltip("60 / shoot interval in seconds")]
		public ushort roundsPerMinute = 100;

		[Tooltip("Muzzle velocity in m/s")]
		public float muzzleVelocity = 600;

		[Tooltip("How much trigger pressure has to be applied to fire")]
		[Range(0.0f, 1.0f)] public float fireTriggerPressure = 0.5f;
		[Range(0.0f, 1.0f)] public float resetTriggerPressure = 0.495f;


		[Header("Feedback")]
		public HapticFeedback fireHapticFeedback = new HapticFeedback(0.75f, 0.05f);
		public RecoilInfo recoilInfo;

		[Header("Audio")]
		public AudioClip shootSound;
		public AudioClip rackSound;
		public AudioClip rackBackSound;
		public AudioClip dryFireSound;

		[Header("Referenes")]
		public GameObject prefab;

		public CartridgeData cartridgeData;

		[SerializeField] public MagazineData[] compatibleMagazines;

		public float SecondsPerRound => 60.0f / roundsPerMinute;
	}
}
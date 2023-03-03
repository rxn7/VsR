using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/WeaponData", fileName = "weapon")]
	public class WeaponData : ScriptableObject {
		public enum FireMode : byte {
			Automatic,
			SemiAutomatic,
			Manual,
		}

		[Header("General")]
		public string displayName;
		public FireMode fireMode;
		[Range(0.0f, 1.0f)] public float fireTriggerValue = 0.3f;

		[Space]
		[Header("Statistics")]
		public float force = 100;

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
		public BulletData bullet;
		public MagazineData[] compatibleMagazines;
	}
}
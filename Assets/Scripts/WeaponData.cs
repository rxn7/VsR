using UnityEngine;

[CreateAssetMenu(menuName = "VsR/Create new Weapon", fileName = "weapon")]
public class WeaponData : ScriptableObject {
	public enum WeaponType : byte {
		Automatic,
		SemiAutomatic,
		Manual,
	}

	[Header("General")]
	public string displayName;
	public WeaponType type;
	[Range(0.0f, 1.0f)] public float fireTriggerValue = 0.3f;

	[Space]
	[Header("Statistics")]
	public float force = 100;
	public ushort ammoPerMag = 10;

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

	[Space]
	[Header("Prefabs")]
	public GameObject prefab;
	public Bullet bulletPrefab;
}

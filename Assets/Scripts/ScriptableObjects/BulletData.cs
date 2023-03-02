using UnityEngine;

[CreateAssetMenu(menuName = "VsR/BulletData", fileName = "bullet")]
public class BulletData : ScriptableObject {
	[Header("General")]
	public string displayName;

	[Header("Stats")]
	public float caliberMm = 9;

	[Header("References")]
	public Bullet bulletPrefab;
	public BulletCase bulletCasePrefab;
}
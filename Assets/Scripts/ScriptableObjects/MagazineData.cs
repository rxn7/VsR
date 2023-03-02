using UnityEngine;

[CreateAssetMenu(menuName = "VsR/MagazineData", fileName = "magazine")]
public class MagazineData : ScriptableObject {
	[Header("Stats")]
	public uint capacity;

	[Header("References")]
	public Magazine prefab;
}
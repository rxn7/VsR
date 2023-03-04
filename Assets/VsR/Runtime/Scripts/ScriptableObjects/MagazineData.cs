using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/MagazineData", fileName = "magazine")]
	public class MagazineData : ScriptableObject {
		[Header("General")]
		public string displayName;

		[Header("Stats")]
		public uint capacity;

		[Header("Audio")]
		public AudioClip slideInSound;
		public AudioClip slideOutSound;

		[Header("References")]
		public Magazine prefab;
		public CartridgeData cartridgeData;
	}
}
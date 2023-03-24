using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/MagazineData", fileName = "magazine")]
	[System.Serializable]
	public class MagazineData : ScriptableObject {
		[Header("General")]
		public CreditsData creditsData;
		public uint capacity;

		[Header("Audio")]
		public AudioClip slideInSound;
		public AudioClip slideOutSound;

		[Header("References")]
		public Magazine prefab;
		public CartridgeData cartridgeData;
	}
}
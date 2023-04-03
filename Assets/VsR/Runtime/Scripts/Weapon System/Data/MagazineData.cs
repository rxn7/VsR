using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/MagazineData", fileName = "magazine")]
	[System.Serializable]
	public class MagazineData : Item {
		[Header("General")]
		public CreditsData creditsData;
		public uint capacity;

		[Header("Audio")]
		public AudioClip slideInSound;
		public AudioClip slideOutSound;

		[Header("References")]
		public CartridgeData cartridgeData;
	}
}
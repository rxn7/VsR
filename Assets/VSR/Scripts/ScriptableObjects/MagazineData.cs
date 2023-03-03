using UnityEngine;

namespace VsR {
	[CreateAssetMenu(menuName = "VsR/MagazineData", fileName = "magazine")]
	public class MagazineData : ScriptableObject {
		[Header("Stats")]
		public uint capacity;

		[Header("Audio")]
		public AudioClip slideInClip;
		public AudioClip slideOutClip;

		[Header("References")]
		public Magazine prefab;
	}
}
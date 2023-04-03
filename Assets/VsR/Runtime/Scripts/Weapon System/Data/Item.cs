using UnityEngine;

namespace VsR {
	public class Item : ScriptableObject {
		[Header("Item data")]
		public string displayName;
		public Texture icon;
		public GameObject prefab;
	}
}
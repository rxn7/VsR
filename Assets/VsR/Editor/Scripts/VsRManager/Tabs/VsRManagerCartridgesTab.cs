using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerCartridgesTab : VsRManagerScriptableObjectEditorTab<CartridgeData> {
		private static GameObject s_prefab = null;
		public override string Name => "Cartridges";
		protected override string PrefabPath => null;
		protected override Object Prefab {
			get => s_prefab;
			set => s_prefab = (GameObject)value;
		}
		protected override bool DeleteWithPrefab => false;

		public override void OnEnable() {
			if (!s_prefab)
				s_prefab = Resources.Load<GameObject>("Prefabs/Cartridge");
		}
	}
}

using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerCartridgesTab : VsRManagerScriptableObjectEditorTab<CartridgeData> {
		private static GameObject s_prefab = null;
		public override string Name => "Cartridges";
		protected override Object Prefab => s_prefab;
		protected override bool DeleteWithPrefab => false;

		public override void OnEnable() {
			if (!s_prefab)
				s_prefab = Resources.Load<GameObject>("Prefabs/Cartridge");
		}
	}
}

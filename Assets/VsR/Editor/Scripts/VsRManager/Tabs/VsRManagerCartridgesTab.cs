using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerCartridgesTab : VsRManagerScriptableObjectEditorTab<CartridgeData> {
		public override string Name => "Cartridges";
		protected override Object GetPrefab() => null;

		protected override void BeforeDrawInspector() {
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Open cartridge prefab"))
				AssetDatabase.OpenAsset(SelectedObject.cartridgePrefab);

			if (GUILayout.Button("Open bullet prefab"))
				AssetDatabase.OpenAsset(SelectedObject.bulletPrefab);

			EditorGUILayout.EndHorizontal();
		}
	}
}

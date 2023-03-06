using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerCartridgesTab : VsRManagerScriptableObjectEditorTab<CartridgeData> {
		public override string Name => "Cartridges";
		protected override Object GetPrefab() => null;

		protected override void BeforeDrawInspector() {
			EditorGUILayout.BeginHorizontal();

			// TODO: Preview cartridge mesh and colliders
			// if (GUILayout.Button("Open cartridge prefab"))
			// 	AssetDatabase.OpenAsset(SelectedObject.cartridgePrefab);

			// TODO: Preview bullet mesh and collider
			// if (GUILayout.Button("Open bullet prefab"))
			// 	AssetDatabase.OpenAsset(SelectedObject.bulletPrefab);

			EditorGUILayout.EndHorizontal();
		}
	}
}

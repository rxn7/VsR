using System.IO;
using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerWeaponsTab : VsRManagerScriptableObjectEditorTab<WeaponData> {
		public override string Name => "Weapons";
		protected override Object Prefab => SelectedObject.prefab;
		protected override bool DeleteWithPrefab => true;

		protected override void OnObjectSelected() {
			base.OnObjectSelected();
			if (SelectedObject.prefab) {
				AssetDatabase.OpenAsset(SelectedObject.prefab);
			} else {
				string folderPath = "Assets/VsR/Runtime/Resources/Prefabs/Weapons";
				Directory.CreateDirectory(folderPath);

				GameObject obj = new GameObject(SelectedObject.name, typeof(Weapon));
				obj.layer = LayerMask.GetMask("Grab");

				SelectedObject.prefab = PrefabUtility.SaveAsPrefabAsset(obj, $"{folderPath}/{SelectedObject.name}.prefab");
				AssetDatabase.OpenAsset(SelectedObject.prefab);
			}
		}

		public override void OnEnable() {
		}
	}
}

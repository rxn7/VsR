using UnityEngine;
using UnityEditor;
using System.IO;

namespace VsR.Editors {
	public class VsRManagerMagazinesTab : VsRManagerScriptableObjectEditorTab<MagazineData> {
		public override string Name => "Magazines";
		protected override Object GetPrefab() => SelectedObject.prefab;

		protected override void OnObjectSelected() {
			base.OnObjectSelected();
			// TODO: Abstract it
			if (SelectedObject.prefab) {
				AssetDatabase.OpenAsset(SelectedObject.prefab);
			} else {
				string folderPath = "Assets/VsR/Runtime/Resources/Prefabs/Magazines";
				Directory.CreateDirectory(folderPath);

				GameObject obj = new GameObject(SelectedObject.name, typeof(Magazine));

				SelectedObject.prefab = PrefabUtility.SaveAsPrefabAsset(obj, $"{folderPath}/{SelectedObject.name}.prefab").GetComponent<Magazine>();
				AssetDatabase.OpenAsset(SelectedObject.prefab);
			}
		}
	}
}

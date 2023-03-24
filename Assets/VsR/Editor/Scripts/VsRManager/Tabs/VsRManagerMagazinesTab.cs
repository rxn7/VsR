using UnityEngine;
using UnityEditor;
using System.IO;

namespace VsR.Editors {
	public class VsRManagerMagazinesTab : VsRManagerScriptableObjectEditorTab<MagazineData> {
		public override string Name => "Magazines";

		protected override Object Prefab {
			get => SelectedObject.prefab;
			set => SelectedObject.prefab = (Magazine)value;
		}

		protected override string PrefabPath => "Assets/VsR/Runtime/Resources/Prefabs/Magazines";
		protected override bool DeleteWithPrefab => true;

		protected override void OnObjectSelected() {
			base.OnObjectSelected();
			// TODO: Abstract it
			if (SelectedObject.prefab) {
				AssetDatabase.OpenAsset(SelectedObject.prefab);
			} else {
			}
		}

		public override void OnEnable() {
		}
	}
}

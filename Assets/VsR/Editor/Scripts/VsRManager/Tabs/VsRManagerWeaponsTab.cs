using System.IO;
using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerWeaponsTab : VsRManagerScriptableObjectEditorTab<WeaponData> {
		public override string Name => "Weapons";

		protected override Object Prefab {
			get => SelectedObject.prefab;
			set => SelectedObject.prefab = (GameObject)value;
		}

		protected override string PrefabPath => "Assets/VsR/Runtime/Resources/Prefabs/Weapons";
		protected override bool DeleteWithPrefab => true;

		protected override void OnObjectSelected() {
			base.OnObjectSelected();
			if (SelectedObject.prefab) {
				AssetDatabase.OpenAsset(SelectedObject.prefab);
			}
		}

		public override void OnEnable() {
		}
	}
}

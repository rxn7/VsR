using System.IO;
using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerWeaponsTab : VsRManagerScriptableObjectEditorTab<WeaponData> {
		public override string Name => "Weapons";

		protected override GameObject Prefab {
			get => SelectedObject.prefab;
			set => SelectedObject.prefab = value;
		}

		protected override string PrefabPath => "Assets/VsR/Runtime/Resources/Prefabs/Weapons";
		protected override bool DeleteWithPrefab => true;

		public override void OnEnable() {
		}
	}
}

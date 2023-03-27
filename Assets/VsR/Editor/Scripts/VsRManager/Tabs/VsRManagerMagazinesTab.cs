using UnityEngine;

namespace VsR.Editors {
	public class VsRManagerMagazinesTab : VsRManagerScriptableObjectEditorTab<MagazineData> {
		public override string Name => "Magazines";

		protected override GameObject Prefab {
			get => SelectedObject.prefab;
			set => SelectedObject.prefab = value;
		}

		protected override string PrefabPath => "Assets/VsR/Runtime/Resources/Prefabs/Magazines";
		protected override bool DeleteWithPrefab => true;

		public override void OnEnable() {
		}
	}
}

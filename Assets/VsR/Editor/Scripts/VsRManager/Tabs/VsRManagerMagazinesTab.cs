using UnityEngine;

namespace VsR.Editors {
	public class VsRManagerMagazinesTab : VsRManagerScriptableObjectEditorTab<MagazineData> {
		public override string Name => "Magazines";
		protected override Object GetPrefab() => SelectedObject.prefab.gameObject;
	}
}

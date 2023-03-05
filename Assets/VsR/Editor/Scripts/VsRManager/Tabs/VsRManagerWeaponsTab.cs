using UnityEngine;

namespace VsR.Editors {
	public class VsRManagerWeaponsTab : VsRManagerScriptableObjectEditorTab<WeaponData> {
		public override string Name => "Weapons";
		protected override Object GetPrefab() => SelectedObject.prefab;
	}
}

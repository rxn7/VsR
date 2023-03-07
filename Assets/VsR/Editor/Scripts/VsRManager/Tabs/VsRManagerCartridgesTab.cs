using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public class VsRManagerCartridgesTab : VsRManagerScriptableObjectEditorTab<CartridgeData> {
		public override string Name => "Cartridges";
		protected override Object GetPrefab() => SelectedObject.cartridgePrefab;
	}
}

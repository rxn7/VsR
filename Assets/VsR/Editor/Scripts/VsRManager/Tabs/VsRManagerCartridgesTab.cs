using UnityEngine;
using UnityEditor;
using System.IO;

namespace VsR.Editors {
	public class VsRManagerCartridgesTab : VsRManagerScriptableObjectEditorTab<CartridgeData> {
		public override string Name => "Cartridges";
		protected override Object GetPrefab() => null;
	}
}

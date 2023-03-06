using UnityEditor;
using UnityEngine;
using System.IO;

namespace VsR.Editors {
	public static class EditorHelper {
		public static void CreateAssetSafe(Object asset, string path) {
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.Refresh();
		}
	}
}
using UnityEditor;
using UnityEngine;
using System.IO;

namespace VsR.Editors {
	public static class EditorHelper {
		public static void CreateAssetSafe(Object asset, string path) {
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			if (File.Exists(path))
				throw new System.Exception($"Cannot create asset on path: {path}, because file with this path already exists!");

			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.Refresh();
		}
	}
}
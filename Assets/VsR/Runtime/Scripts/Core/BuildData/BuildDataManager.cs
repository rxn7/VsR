using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace VsR {
	public static class BuildDataManager {
		public static BuildData Data { get; private set; }

		[RuntimeInitializeOnLoadMethod]
		private static void Init() {
			TextAsset asset = Resources.Load<TextAsset>("build_data");
			Data = JsonUtility.FromJson<BuildData>(asset.text);
		}
	}
}

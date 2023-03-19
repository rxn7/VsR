using System.IO;
using UnityEngine;

namespace VsR {
	public static class BuildDataManager {
		public static BuildData Data { get; private set; }

		[RuntimeInitializeOnLoadMethod]
		private static void Init() {
			string path = Path.Combine(Application.dataPath, "build_data.json");
			string json = File.ReadAllText(path);

			Data = JsonUtility.FromJson<BuildData>(json);
		}
	}
}

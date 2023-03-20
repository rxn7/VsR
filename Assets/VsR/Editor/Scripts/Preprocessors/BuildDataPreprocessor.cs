using System.IO;
using UnityEngine;

namespace VsR.Editors {
	public class BuildDataPreprocessor {
		[UnityEditor.Callbacks.DidReloadScripts]
		public static void SaveBuildData() {
			BuildData data = CreateBuildData();

			string dirPath = Path.Combine(Application.dataPath, "Resources");
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);

			File.WriteAllText(Path.Combine(dirPath, "build_data.json"), JsonUtility.ToJson(data));
		}

		public static BuildData CreateBuildData() {
			return new BuildData {
				gitBranch = ProcessHelper.GetProcessOutputLine("git", "branch --show-current"),
				gitCommit = ProcessHelper.GetProcessOutputLine("git", "log -1 --pretty=format:%h"),
				buildTimeUTC = System.DateTime.UtcNow.ToString(),
				version = Application.version,
			};
		}
	}
}
using System.IO;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace VsR.Editors {
	public class BuildDataPreprocessor : IPreprocessBuildWithReport {
		public int callbackOrder => 1;

		public void OnPreprocessBuild(BuildReport report) {
			Debug.Log(Path.GetDirectoryName(report.summary.outputPath));
			SaveBuildData(Path.GetDirectoryName(report.summary.outputPath));
		}

		public static void SaveBuildData(string dataPath = "") {
			if (dataPath == "")
				dataPath = Application.dataPath;

			string path = dataPath + "/build_data.json";
			BuildData data = CreateBuildData();

			Debug.Log(path);

			File.WriteAllText(path, JsonUtility.ToJson(data));
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReload() {
			SaveBuildData();
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
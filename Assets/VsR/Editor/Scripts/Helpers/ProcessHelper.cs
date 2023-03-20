using System.Diagnostics;

namespace VsR.Editors {
	public static class ProcessHelper {
		public static string GetProcessOutputLine(string exe, string args, bool waitForExit = true) {
			Process process = new Process();
			process.StartInfo.FileName = exe;
			process.StartInfo.Arguments = args;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.Start();

			string output = process.StandardOutput.ReadLine();

			if (waitForExit)
				process.WaitForExit();

			return output;
		}
	}
}
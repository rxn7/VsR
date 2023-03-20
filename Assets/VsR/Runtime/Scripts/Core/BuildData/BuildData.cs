namespace VsR {
	[System.Serializable]
	public struct BuildData {
		public string gitBranch;
		public string gitCommit;
		public string buildTimeUTC;
		public string version;
	}
}

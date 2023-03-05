using UnityEngine;

namespace VsR.Editors {
	public abstract class VsRManagerTab {
		public abstract string Name { get; }
		public abstract void Draw();
	}
}
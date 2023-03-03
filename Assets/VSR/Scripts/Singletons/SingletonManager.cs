using UnityEngine;

namespace VsR {
	public class SingletonManager : SingletonBehaviour<SingletonManager> {
		protected override void Awake() {
			base.Awake();
			SoundManager.CreateInstance();
		}
	}
}
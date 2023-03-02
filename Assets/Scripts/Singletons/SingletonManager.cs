using UnityEngine;

public class SingletonManager : MonoBehaviour {
	private static SingletonManager s_instance = null;

	public void Awake() {
		if (s_instance != null) {
			if (s_instance != this)
				throw new System.Exception($"There can only be one instance of {nameof(SingletonManager)}");
			return;
		}

		s_instance = this;

		SoundManager.Init();
	}
}
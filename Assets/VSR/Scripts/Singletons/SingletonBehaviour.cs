using UnityEngine;

namespace VsR {
	// TODO: Thread safety if needed
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {
		private static T s_instance;
		public static T Instance => (T)s_instance;

		public static GameObject CreateInstance() => new GameObject(typeof(T).Name, typeof(T));

		protected virtual void Awake() {
			if (s_instance != null && s_instance != this) {
				Debug.LogError($"Multiple instances of {typeof(T).FullName} are not allowed!");
				Destroy(gameObject);
				return;
			}

			s_instance = (T)this;
			DontDestroyOnLoad(this);
		}
	}
}
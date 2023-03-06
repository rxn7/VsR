using UnityEngine;
using UnityEngine.Pool;

namespace VsR {
	public class SoundPoolManager : SingletonBehaviour<SoundPoolManager> {
		[RuntimeInitializeOnLoadMethod] private static void _CreateInstance() => CreateInstance();

		private ObjectPool<AudioSource> m_pool;

		protected override void Awake() {
			base.Awake();
			m_pool = new ObjectPool<AudioSource>(CreatePooledObject, null, null, null, true, 20, 50);
		}

		protected AudioSource CreatePooledObject() {
			AudioSource source = new GameObject($"AudioSource").AddComponent<AudioSource>();
			source.transform.SetParent(transform);
			source.playOnAwake = false;
			source.loop = false;
			source.rolloffMode = AudioRolloffMode.Linear;
			return source;
		}

		public void PlaySound(AudioClip clip, Vector3 position, float pitch = 1.0f, float volume = 1.0f, float spatialBlend = 1.0f) {
			if (!clip)
				return;

			AudioSource source = m_pool.Get();
			source.pitch = pitch;
			source.transform.position = position;
			source.spatialBlend = spatialBlend;
			source.PlayOneShot(clip, volume);
		}
	}
}
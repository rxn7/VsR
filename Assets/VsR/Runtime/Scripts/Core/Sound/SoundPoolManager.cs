using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

namespace VsR {
	public class SoundPoolManager : SingletonBehaviour<SoundPoolManager> {
		[RuntimeInitializeOnLoadMethod] private static void _CreateInstance() => CreateInstance();

		private ObjectPool<AudioSource> m_pool;

		protected override void Awake() {
			base.Awake();
			m_pool = new ObjectPool<AudioSource>(CreatePooledObject,
				(AudioSource src) => src.enabled = true,
				(AudioSource src) => src.enabled = false,
				(AudioSource src) => Destroy(src.gameObject),
				true, 20, 50);
		}

		protected AudioSource CreatePooledObject() {
			AudioSource source = new GameObject($"AudioSource").AddComponent<AudioSource>();
			source.transform.SetParent(transform);
			source.playOnAwake = false;
			source.loop = false;
			source.rolloffMode = AudioRolloffMode.Linear;
			return source;
		}

		public void PlaySound(AudioClip clip, Vector3 position, float pitch = 1.0f, float volume = 1.0f, float spatialBlend = 1.0f, bool scaledWithTimeScale = true) {
			if (!clip)
				return;

			if (scaledWithTimeScale)
				pitch *= Time.timeScale;

			AudioSource source = m_pool.Get();
			source.pitch = pitch;
			source.transform.position = position;
			source.spatialBlend = spatialBlend;
			source.PlayOneShot(clip, volume);

			StartCoroutine(ReleaseAudioSourceAfterFinishedPlaying(source, clip.length / pitch));
		}

		private IEnumerator ReleaseAudioSourceAfterFinishedPlaying(AudioSource source, float duration) {
			yield return new WaitForSeconds(duration);
			m_pool.Release(source);
		}
	}
}
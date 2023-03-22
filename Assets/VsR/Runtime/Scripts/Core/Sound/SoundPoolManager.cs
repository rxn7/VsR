using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

namespace VsR {
	public class SoundPoolManager : SingletonBehaviour<SoundPoolManager> {
		private ObjectPool<Sound> m_pool;
		private int m_activeAudioSourceCount = 0;

		protected override void Awake() {
			base.Awake();
			m_pool = new ObjectPool<Sound>(CreateSound,
				(Sound snd) => snd.enabled = true,
				(Sound snd) => snd.enabled = false,
				(Sound snd) => Destroy(snd.gameObject),
				true, 20, 150);
		}

		protected Sound CreateSound() {
			Sound snd = new GameObject("Sound").AddComponent<Sound>();
			snd.transform.SetParent(transform);
			return snd;
		}

		public void PlaySound(AudioClip clip, Vector3 position, float pitch = 1.0f, float volume = 1.0f, float spatialBlend = 1.0f) {
			if (!clip)
				return;

			Sound snd = m_pool.Get();
			snd.Play(clip, position, pitch, volume, spatialBlend);

			m_activeAudioSourceCount++;

			StartCoroutine(ReleaseSoundAfterFinishedPlaying(snd));
		}

		public int ActiveAudioSourceCount => m_activeAudioSourceCount;
		public int AllAudioSourceCount => m_pool.CountAll;

		private IEnumerator ReleaseSoundAfterFinishedPlaying(Sound snd) {
			yield return new WaitUntil(() => !snd.IsPlaying);
			m_activeAudioSourceCount--;
			m_pool.Release(snd);
		}
	}
}
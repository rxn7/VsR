using System.Collections.Generic;
using UnityEngine;

namespace VsR {
	public class SoundManager : SingletonBehaviour<SoundManager> {
		public const uint SOUND_POOL_CAPACITY = 20;
		private AudioSource[] m_sources;
		private Queue<AudioSource> m_freeSources = new Queue<AudioSource>(20);

		protected override void Awake() {
			base.Awake();

			Transform parent = new GameObject("SoundManager").transform;
			m_sources = new AudioSource[SOUND_POOL_CAPACITY];
			for (uint i = 0; i < SOUND_POOL_CAPACITY; ++i) {
				AudioSource source = new GameObject("Sound").AddComponent<AudioSource>();
				source.transform.parent = parent;
				source.playOnAwake = false;
				source.loop = false;
				source.rolloffMode = AudioRolloffMode.Logarithmic;
				source.spatialBlend = 1.0f; // 3D effect

				m_sources[i] = source;
				m_freeSources.Enqueue(source);
			}
		}

		private AudioSource GetFreeAudioSource() {
			if (m_freeSources.Count == 0) {
				Debug.LogWarning("There are no free AudioSources! Consider bumping up the SOUND_POOL_CAPACITY!");
				return m_sources[0];
			}

			return m_freeSources.Dequeue();
		}

		public void PlaySound(AudioClip clip, Vector3 position, float pitch = 1.0f, float volume = 1.0f) {
			if (clip == null)
				return;

			AudioSource source = GetFreeAudioSource();
			source.pitch = pitch;
			source.transform.position = position;
			source.PlayOneShot(clip, volume);

			Invoke("EnqueueFreeAudioSource", clip.length / pitch);
		}

		private void EnqueueFreeAudioSource(AudioSource source) {
			m_freeSources.Enqueue(source);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VsR {
	public class Sound : MonoBehaviour {
		private AudioSource m_source;
		private float m_initPitch = 1.0f;

		public bool IsPlaying => m_source.isPlaying;

		private void Awake() {
			m_source = gameObject.AddComponent<AudioSource>();
			m_source.playOnAwake = false;
			m_source.loop = false;
			m_source.rolloffMode = AudioRolloffMode.Logarithmic;

			TimeManager.onTimeScaleChanged += (float scale) => m_source.pitch = m_initPitch * scale;
		}

		public void Play(AudioClip clip, Vector3 position, float pitch = 1.0f, float volume = 1.0f, float spatialBlend = 1.0f) {
			m_initPitch = pitch;

			m_source.transform.position = position;
			m_source.volume = volume;
			m_source.spatialBlend = spatialBlend;
			m_source.pitch = pitch * TimeManager.TimeScale;

			m_source.PlayOneShot(clip);
		}
	}
}

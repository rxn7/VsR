using UnityEngine;

public static class SoundManager {
	public const uint SOUND_QUEUE_CAPACITY = 20;
	private static AudioSource[] s_sources;

	public static void Init() {
		Transform parent = new GameObject("SoundManager").transform;
		s_sources = new AudioSource[SOUND_QUEUE_CAPACITY];
		for (uint i = 0; i < SOUND_QUEUE_CAPACITY; ++i) {
			AudioSource source = new GameObject("Sound").AddComponent<AudioSource>();
			source.transform.parent = parent;
			source.playOnAwake = false;
			source.loop = false;
			source.rolloffMode = AudioRolloffMode.Logarithmic;
			source.spatialBlend = 1.0f; // 3D effect
			s_sources[i] = source;
		}
	}

	private static AudioSource GetFreeAudioSource() {
		foreach (AudioSource source in s_sources) {
			if (!source.isPlaying)
				return source;
		}

		return s_sources[s_sources.Length - 1];
	}

	public static void PlaySound(AudioClip clip, Vector3 position, float pitch = 1.0f, float volume = 1.0f) {
		if (clip == null)
			return;

		AudioSource source = GetFreeAudioSource();
		source.pitch = pitch;
		source.transform.position = position;
		source.volume = volume;
		source.PlayOneShot(clip);
	}
}
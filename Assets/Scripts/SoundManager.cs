using UnityEngine;

public class SoundManager : MonoBehaviour {
	public const uint SOUND_QUEUE_CAPACITY = 20;

	private static SoundManager s_instance;
	public static SoundManager Instance {
		get {
			if (!s_instance) {
				s_instance = new GameObject("SoundManager").AddComponent<SoundManager>();
				DontDestroyOnLoad(s_instance);
			}

			return s_instance;
		}
	}

	private AudioSource[] s_sources;

	private void Awake() {
		s_sources = new AudioSource[SOUND_QUEUE_CAPACITY];
		for (uint i = 0; i < SOUND_QUEUE_CAPACITY; ++i) {
			AudioSource source = new GameObject("Sound").AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.loop = false;
			source.rolloffMode = AudioRolloffMode.Logarithmic;
			source.spatialBlend = 1.0f; // 3D effect
			s_sources[i] = source;
		}
	}

	private static AudioSource GetFreeAudioSource() {
		foreach (AudioSource source in Instance.s_sources) {
			if (!source.isPlaying)
				return source;
		}

		return null;
	}

	public static void PlaySound(AudioClip clip, Vector3 position, float pitch = 1.0f) {
		AudioSource source = GetFreeAudioSource();
		source.pitch = pitch;
		source.transform.position = position;
		source.PlayOneShot(clip);
	}
}
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
			s_sources[i] = new GameObject("Sound").AddComponent<AudioSource>();
			s_sources[i].playOnAwake = false;
			s_sources[i].loop = false;
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
		source.rolloffMode = AudioRolloffMode.Logarithmic;
		source.PlayOneShot(clip);
	}
}
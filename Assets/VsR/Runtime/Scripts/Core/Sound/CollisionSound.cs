using UnityEngine;

namespace VsR {
	[DisallowMultipleComponent]
	public class CollisionSound : MonoBehaviour {
		[SerializeField] public AudioClip[] m_collisionSounds;
		[SerializeField] private float m_minTimeBetweenSounds = 0.1f;
		[SerializeField] private Math.FloatRange m_pitchRange = new Math.FloatRange(0.8f, 1.5f);
		[SerializeField] private Math.FloatRange m_volumeRange = new Math.FloatRange(0.2f, 1.0f);
		[SerializeField] private float m_volumeMultiplier = 0.2f;
		[SerializeField] private float m_pitchMultiplier = 1.0f;

		private float m_lastCollisionTime;
		private AudioClip GetRandomCollisionSound() => m_collisionSounds.Length != 0 ? m_collisionSounds[Random.Range(0, m_collisionSounds.Length)] : null;

		private void Awake() {
			m_lastCollisionTime = Time.time;
		}

		private void OnCollisionEnter(Collision collision) {
			if (Time.time - m_lastCollisionTime < m_minTimeBetweenSounds)
				return;

			float velocity = collision.relativeVelocity.magnitude;
			float pitch = m_pitchRange.Clamp(velocity * m_pitchMultiplier);
			float volume = m_volumeRange.Clamp(velocity * m_volumeMultiplier);

			SoundPoolManager.Instance.PlaySound(GetRandomCollisionSound(), transform.position, pitch, volume);
			m_lastCollisionTime = Time.time;
		}
	}
}

using UnityEngine;

namespace VsR {
	public class CollisionSound : MonoBehaviour {
		[SerializeField] public AudioClip[] m_collisionSounds;

		private AudioClip GetRandomCollisionSound() => m_collisionSounds.Length != 0 ? m_collisionSounds[Random.Range(0, m_collisionSounds.Length)] : null;

		private void OnCollisionEnter(Collision collision) {
			float velocity = collision.relativeVelocity.magnitude * 0.2f;

			float pitch = Mathf.Clamp(velocity, 0.5f, 1.5f);
			float volume = Mathf.Clamp01(velocity);

			SoundPoolManager.Instance.PlaySound(GetRandomCollisionSound(), transform.position, pitch, volume);
		}
	}
}

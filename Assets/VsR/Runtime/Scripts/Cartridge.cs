using UnityEngine;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	public class Cartridge : MonoBehaviour {
		public const float EJECT_LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_bulletMeshRenderer;
		private Rigidbody m_rb;


		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
			m_rb.interpolation = RigidbodyInterpolation.Interpolate;
			m_rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}

		public void Eject(bool withBullet = false, float force = 1.0f) {
			m_rb.AddForce(transform.up * force, ForceMode.Impulse);
			m_bulletMeshRenderer.enabled = withBullet;

			Destroy(gameObject, EJECT_LIFE_TIME_SECS);
		}
	}
}
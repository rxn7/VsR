using UnityEngine;
using UnityEngine.Pool;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	public class Cartridge : MonoBehaviour {
		public const float EJECT_LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_bulletMeshRenderer;
		[SerializeField] private MeshFilter m_cartridgeMeshFilter;
		private Rigidbody m_rb;

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
			m_rb.interpolation = RigidbodyInterpolation.Interpolate;
			m_rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}

		public void Eject(bool withBullet = false, float force = 1.0f) {
			EnablePhysics();
			m_rb.AddForce(transform.up * force, ForceMode.Impulse);
			m_bulletMeshRenderer.enabled = withBullet;
		}

		public void DisablePhysics() {
			m_rb.detectCollisions = false;
			m_rb.isKinematic = true;
			m_rb.useGravity = false;
		}

		public void EnablePhysics() {
			m_rb.detectCollisions = true;
			m_rb.isKinematic = false;
			m_rb.useGravity = true;
		}
	}
}
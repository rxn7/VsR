using UnityEngine;
using VsR.Math;

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

		public void Eject(WeaponData weaponData, bool withBullet = false) {
			Vector3 random = VectorHelper.RandomVector(new FloatRange(0.5f, 1.5f), new FloatRange(0.5f, 1.5f), new FloatRange(0.5f, 1.5f));
			Vector3 randomAngular = VectorHelper.RandomVector(new FloatRange(0.5f, 1.5f), new FloatRange(0.5f, 1.5f), new FloatRange(0.5f, 1.5f));

			EnablePhysics();
			m_rb.AddForce(Vector3.Scale(transform.up, random) * weaponData.cartridgeEjectForce * Random.Range(0.9f, 1.1f), ForceMode.Impulse);
			m_rb.AddTorque(Vector3.Scale(-transform.right, randomAngular) * weaponData.cartridgeEjectTorque, ForceMode.Impulse);
			m_bulletMeshRenderer.enabled = withBullet;
			Destroy(gameObject, EJECT_LIFE_TIME_SECS);
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
using UnityEngine;
using VsR.Math;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CollisionSound))]
	[ExecuteAlways]
	public class Cartridge : MonoBehaviour {
		public const float EJECT_LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_bulletMeshRenderer;
		[SerializeField] private MeshFilter m_bulletMeshFilter;
		[SerializeField] private MeshFilter m_cartridgeMeshFilter;
		[SerializeField] private CapsuleCollider m_collider;
		private Rigidbody m_rb;
		private CollisionSound m_collisionSound;
		private CartridgeData m_data;

		private void OnEnable() {
			m_rb = GetComponent<Rigidbody>();
			m_rb.interpolation = RigidbodyInterpolation.Interpolate;
			m_collisionSound = GetComponent<CollisionSound>();
		}

		public void Setup(CartridgeData data, bool withBullet) {
			m_data = data;

			m_cartridgeMeshFilter.mesh = data.cartridgeMesh;
			m_bulletMeshFilter.mesh = data.bulletMesh;
			m_bulletMeshRenderer.enabled = withBullet;

			m_collider.height = data.cartridgeLengthMm * 0.001f;
			m_collider.radius = data.cartridgeRadiusMm * 0.0005f;
			m_collider.center = Vector3.back * m_collider.height * 0.5f;
		}

		public void Eject(Weapon weapon, float force, float torque, FloatRange randomness, bool withBullet = false) {
			CancelInvoke();
			Setup(weapon.Data.cartridgeData, withBullet);
			m_collisionSound.m_collisionSounds = m_data.cartridgeCollisionSounds;

			Vector3 random = new Vector3(randomness.RandomValue(), randomness.RandomValue(), randomness.RandomValue());
			Vector3 randomAngular = new Vector3(randomness.RandomValue(), randomness.RandomValue(), randomness.RandomValue());

			transform.position = weapon.CartridgeEjectPoint.position;
			transform.rotation = weapon.CartridgeEjectPoint.rotation;

			m_rb.position = transform.position;
			m_rb.velocity = Vector3.Scale(transform.up, random) * force + weapon.WorldVelocity;
			m_rb.angularVelocity = Vector3.Scale(-transform.right, randomAngular) * torque;

			Invoke(nameof(Release), EJECT_LIFE_TIME_SECS);
		}

		private void Release() {
			CancelInvoke();
			CartridgePoolManager.Pool.Release(this);
		}

		public void OnGet() {
			gameObject.SetActive(true);
		}

		public void OnRelease() {
			gameObject.SetActive(false);
		}
	}
}
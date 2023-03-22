using UnityEngine;
using VsR.Math;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CollisionSound))]
	public class Cartridge : MonoBehaviour {
		public const float EJECT_LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_bulletMeshRenderer;
		[SerializeField] private MeshFilter m_bulletMeshFilter;
		[SerializeField] private MeshFilter m_cartridgeMeshFilter;
		private Rigidbody m_rb;
		private CollisionSound m_collisionSound;
		private CartridgeData m_data;

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
			m_collisionSound = GetComponent<CollisionSound>();
		}

		public void Eject(Weapon weapon, float force, float torque, FloatRange randomness, bool withBullet = false) {
			CancelInvoke();
			m_data = weapon.Data.cartridgeData;
			m_collisionSound.m_collisionSounds = m_data.cartridgeCollisionSounds;

			m_cartridgeMeshFilter.mesh = weapon.Data.cartridgeData.cartridgeMesh;
			m_bulletMeshFilter.mesh = weapon.Data.cartridgeData.bulletMesh;
			m_bulletMeshRenderer.enabled = withBullet;

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
			CartridgePoolManager.Instance.Pool.Release(this);
		}

		public void OnGet() {
			gameObject.SetActive(true);
		}

		public void OnRelease() {
			gameObject.SetActive(false);
		}
	}
}
using UnityEngine;
using VsR.Math;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	public class Cartridge : MonoBehaviour {
		public const float EJECT_LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_bulletMeshRenderer;
		[SerializeField] private MeshFilter m_bulletMeshFilter;
		[SerializeField] private MeshFilter m_cartridgeMeshFilter;
		private Rigidbody m_rb;
		private bool m_ejected = false;
		private CartridgeData m_data;

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
		}

		public void Eject(Weapon weapon, float force, float torque, FloatRange randomness, bool withBullet = false) {
			CancelInvoke();

			m_ejected = true;
			m_data = weapon.Data.cartridgeData;

			m_cartridgeMeshFilter.mesh = weapon.Data.cartridgeData.cartridgeMesh;
			m_bulletMeshFilter.mesh = weapon.Data.cartridgeData.bulletMesh;
			m_bulletMeshRenderer.enabled = withBullet;

			Vector3 random = VectorHelper.RandomVector(randomness, randomness, randomness);
			Vector3 randomAngular = VectorHelper.RandomVector(randomness, randomness, randomness);

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
			m_ejected = false;
			gameObject.SetActive(false);
		}

		private void OnCollisionEnter(Collision collision) {
			if (!m_ejected)
				return;

			float velocity = collision.relativeVelocity.magnitude * 0.2f;

			float pitch = Mathf.Clamp(velocity, 0.5f, 1.5f);
			float volume = Mathf.Clamp01(velocity);

			SoundPoolManager.Instance.PlaySound(m_data.GetRandomCollideSound(), transform.position, pitch, volume);
		}
	}
}
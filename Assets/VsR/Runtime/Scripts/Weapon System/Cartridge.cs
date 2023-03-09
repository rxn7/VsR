using UnityEngine;
using VsR.Math;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	public class Cartridge : MonoBehaviour {
		public const float EJECT_LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_bulletMeshRenderer;
		[SerializeField] private MeshFilter m_cartridgeMeshFilter;
		private Rigidbody m_rb;
		private bool m_ejected = false;
		private CartridgeData m_data;

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
			m_rb.interpolation = RigidbodyInterpolation.Interpolate;
			m_rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}

		public void Eject(WeaponData weaponData, Transform ejectPoint, bool withBullet = false) {
			m_ejected = true;
			m_data = weaponData.cartridgeData;
			m_cartridgeMeshFilter.mesh = weaponData.cartridgeData.cartridgeMesh;
			m_bulletMeshRenderer.enabled = withBullet;

			transform.SetPositionAndRotation(ejectPoint.position, ejectPoint.rotation);

			FloatRange randomRange = new FloatRange(0.75f, 1.25f);
			Vector3 random = VectorHelper.RandomVector(randomRange, randomRange, randomRange);
			Vector3 randomAngular = VectorHelper.RandomVector(randomRange, randomRange, randomRange);

			EnablePhysics();
			m_rb.AddForce(Vector3.Scale(transform.up, random) * weaponData.cartridgeEjectForce, ForceMode.Impulse);
			m_rb.AddTorque(Vector3.Scale(-transform.right, randomAngular) * weaponData.cartridgeEjectTorque, ForceMode.Impulse);

			Invoke(nameof(Release), EJECT_LIFE_TIME_SECS);
		}

		private void Release() {
			CancelInvoke();
			CartridgePoolManager.Instance.Pool.Release(this);
		}

		public void Enable() {
			gameObject.SetActive(true);
			EnablePhysics();
		}

		public void Disable() {
			m_ejected = false;
			CancelInvoke();
			gameObject.SetActive(false);
			DisablePhysics();
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

		private void OnCollisionEnter(Collision collision) {
			if (!m_ejected)
				return;

			SoundPoolManager.Instance.PlaySound(m_data.GetRandomCollideSound(), transform.position, Random.Range(0.9f, 1.1f));
		}
	}
}
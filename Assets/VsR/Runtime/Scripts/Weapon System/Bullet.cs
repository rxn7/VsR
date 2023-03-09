using UnityEngine;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(TrailRenderer))]
	public class Bullet : MonoBehaviour {
		private const float LIFE_TIME_SECS = 10.0f;

		private MeshFilter m_meshFilter;
		private MeshRenderer m_meshRenderer;
		private CapsuleCollider m_collider;
		private TrailRenderer m_trail;
		private Rigidbody m_rb;
		private bool m_collided = false;

		public Collider Collider => m_collider;

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
			m_collider = GetComponent<CapsuleCollider>();
			m_trail = GetComponent<TrailRenderer>();
			m_meshFilter = GetComponentInChildren<MeshFilter>();
			m_meshRenderer = GetComponentInChildren<MeshRenderer>();

			m_rb.isKinematic = true;
			m_rb.interpolation = RigidbodyInterpolation.None;
			m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}

		private void FixedUpdate() {
			if (Mathf.Approximately(m_rb.velocity.sqrMagnitude, 0))
				return;

			transform.rotation = Quaternion.LookRotation(m_rb.velocity);
		}

		public void Setup(WeaponData weaponData) {
			m_collided = false;
			m_rb.mass = weaponData.cartridgeData.bulletMassGrams * 0.001f;
			m_meshFilter.mesh = weaponData.cartridgeData.bulletMesh;
			m_collider.radius = weaponData.cartridgeData.bulletDiameterMm * 0.0005f;
			m_collider.height = weaponData.cartridgeData.bulletLengthMm * 0.001f;
		}

		private void OnCollisionEnter(Collision collision) {
			m_collided = true;

			DisablePhysics();
			Invoke(nameof(Release), LIFE_TIME_SECS);

			IHIttable hittable = collision.collider.GetComponent<IHIttable>();
			hittable?.OnHit(collision);
		}

		public void Fire(Transform barrelEndPoint, WeaponData weaponData) {
			Setup(weaponData);
			transform.SetPositionAndRotation(barrelEndPoint.position, barrelEndPoint.rotation);
			m_trail.Clear();

			Vector3 force = transform.forward * m_rb.mass * weaponData.muzzleVelocity;
			m_rb.AddForce(force, ForceMode.Impulse);

			Invoke(nameof(ReleaseIfNotCollided), LIFE_TIME_SECS);
		}

		private void Release() {
			CancelInvoke();
			BulletPoolManager.Instance.Pool.Release(this);
		}

		public void Enable() {
			gameObject.SetActive(true);
			EnablePhysics();
		}

		public void Disable() {
			gameObject.SetActive(false);
			DisablePhysics();
		}

		public void ReleaseIfNotCollided() {
			if (m_collided)
				return;

			Release();
		}

		public void EnablePhysics() {
			m_rb.detectCollisions = true;
			m_rb.isKinematic = false;
			m_rb.useGravity = true;
		}

		public void DisablePhysics() {
			m_rb.detectCollisions = false;
			m_rb.isKinematic = true;
			m_rb.useGravity = false;
		}

	}
}
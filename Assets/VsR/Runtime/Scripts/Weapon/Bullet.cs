using UnityEngine;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	public class Bullet : MonoBehaviour {
		public const float LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_mesh;
		[SerializeField] private Collider m_collider;
		[SerializeField] private TrailRenderer m_trail;
		private Rigidbody m_rb;

		public Collider Collider => m_collider;

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
			m_rb.isKinematic = true;

			if (!m_collider || !m_rb)
				throw new UnassignedReferenceException("Not all Bullet's references are assigned!");
		}

		private void FixedUpdate() {
			print($"Bullet velocity {m_rb.velocity.magnitude}");
			transform.LookAt(transform.position + transform.forward);
		}

		private void OnCollisionEnter(Collision collision) {
			PseudoDestroy();

			IHIttable hittable = collision.collider.GetComponent<IHIttable>();
			hittable?.OnHit(collision);
		}

		public void Fire(WeaponData weaponData) {
			m_rb.isKinematic = false;

			Vector3 force = transform.forward * m_rb.mass * weaponData.muzzleVelocity;
			m_rb.AddForce(force, ForceMode.Impulse);

			Invoke("PseudoDestroy", LIFE_TIME_SECS);
		}

		private void PseudoDestroy() {
			if (m_trail == null) {
				Destroy(gameObject);
				return;
			}

			m_rb.detectCollisions = false;
			m_rb.isKinematic = true;
			m_rb.useGravity = false;

			if (m_mesh != null)
				m_mesh.enabled = false;

			Destroy(gameObject, m_trail.time);
		}
	}
}
using UnityEngine;

namespace VsR {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(TrailRenderer))]
	public class Bullet : MonoBehaviour {
		public const float LIFE_TIME_SECS = 3.0f;

		[SerializeField] private MeshRenderer m_mesh;
		[SerializeField] private Collider m_collider;
		private Rigidbody m_rb;
		private TrailRenderer m_trailRenderer;

		public Collider Collider => m_collider;

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
			m_rb.isKinematic = true;

			if (!m_collider || !m_rb)
				throw new UnassignedReferenceException("Not all Bullet's references are assigned!");

			m_trailRenderer = GetComponent<TrailRenderer>();
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

		public void ApplyForce(Vector3 direction, float force) {
			m_rb.isKinematic = false;
			transform.LookAt(transform.position + direction);
			m_rb.AddForce(direction * force, ForceMode.Impulse);
			Invoke("PseudoDestroy", LIFE_TIME_SECS);
		}

		private void PseudoDestroy() {
			m_rb.detectCollisions = false;
			m_rb.isKinematic = true;
			m_rb.useGravity = false;

			if (m_mesh != null)
				m_mesh.enabled = false;

			Destroy(gameObject, m_trailRenderer.time);
		}
	}
}
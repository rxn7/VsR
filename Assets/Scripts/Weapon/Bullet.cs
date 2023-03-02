using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
public class Bullet : MonoBehaviour {
	public const float LIFE_TIME_SECS = 3.0f;

	[SerializeField] private MeshRenderer m_mesh;
	private Rigidbody m_rb;
	private TrailRenderer m_trailRenderer;

	private void Awake() {
		m_rb = GetComponent<Rigidbody>();
		m_trailRenderer = GetComponent<TrailRenderer>();
		Invoke("PseudoDestroy", LIFE_TIME_SECS);
	}

	private void FixedUpdate() {
		transform.LookAt(transform.position + transform.forward);
	}

	private void OnCollisionEnter(Collision collision) {
		PseudoDestroy();

		IHIttable hittable = collision.collider.GetComponent<IHIttable>();
		hittable?.OnHit(collision);
	}

	public void ApplyForce(Vector3 direction, float force) {
		transform.LookAt(transform.position + direction);
		m_rb.AddForce(direction * force, ForceMode.Impulse);
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

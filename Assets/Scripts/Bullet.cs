using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
	public const float LIFE_TIME_SECS = 3.0f;
	private Rigidbody m_rb;

	private void Awake() {
		m_rb = GetComponent<Rigidbody>();
		Destroy(gameObject, LIFE_TIME_SECS);
	}

	private void FixedUpdate() {
		transform.LookAt(transform.position + transform.forward);
	}

	private void OnCollisionEnter(Collision collision) {
		Destroy(gameObject);
	}

	public void ApplyForce(Vector3 direction, float force) {
		transform.LookAt(transform.position + direction);
		m_rb.AddForce(direction * force, ForceMode.Impulse);
	}
}

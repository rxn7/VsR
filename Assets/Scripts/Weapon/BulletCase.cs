using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletCase : MonoBehaviour {
	public const float LIFE_TIME_SECS = 3.0f;

	public void Eject(float force = 1.0f) {
		GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
		Destroy(gameObject, LIFE_TIME_SECS);
	}
}

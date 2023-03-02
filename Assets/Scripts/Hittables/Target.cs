using UnityEngine;

public class Target : MonoBehaviour, IHIttable {
	[SerializeField] private AudioClip m_hitSound;
	[SerializeField] private MeshRenderer m_renderer;
	private float m_hitTimer = 0.0f;

	private void Update() {
		if (m_hitTimer > 0.0f) {
			m_renderer.material.color = Color.red;
			m_hitTimer -= Time.deltaTime;
		} else {
			m_renderer.material.color = Color.blue;
		}
	}

	public void OnHit(Collision collision) {
		m_hitTimer = 1.0f;
		SoundManager.PlaySound(m_hitSound, collision.GetContact(0).point, Random.Range(0.9f, 1.1f));
	}
}

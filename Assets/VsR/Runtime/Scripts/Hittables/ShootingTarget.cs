using UnityEngine;

namespace VsR {
	public class ShootingTarget : MonoBehaviour, IHIttable {
		[SerializeField] private MeshRenderer m_renderer;
		private float m_hitTimer = 0.0f;

		private void Update() {
			if (m_hitTimer > 0.0f) {
				m_renderer.material.color = Color.red;
				m_hitTimer -= Time.deltaTime;
			} else {
				m_renderer.material.color = Color.green;
			}
		}

		public void OnHit(Collision collision) {
			m_hitTimer = 1.0f;
		}
	}
}
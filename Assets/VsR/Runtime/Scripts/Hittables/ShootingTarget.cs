using UnityEngine;
using System.Collections;

namespace VsR {
	public class ShootingTarget : MonoBehaviour, IHIttable {
		[SerializeField] private MeshRenderer m_renderer;
		[SerializeField] private AudioClip m_hitSound;
		private Coroutine m_onHitCoroutine = null;

		public void OnHit(Collision collision) {
			SoundManager.Instance.PlaySound(m_hitSound, transform.position, Random.Range(0.95f, 1.05f), 10, 0.5f);
			if (m_onHitCoroutine != null)
				StopCoroutine(m_onHitCoroutine);

			m_onHitCoroutine = StartCoroutine(OnHitCoroutine());
		}

		private IEnumerator OnHitCoroutine() {
			float elapsed = 0.0f;
			while (elapsed < 1.0f) {
				m_renderer.material.color = Color.Lerp(Color.red, Color.yellow, elapsed / 1.0f);
				elapsed += Time.deltaTime;
				yield return null;
			}
			m_renderer.material.color = Color.yellow;
		}
	}
}
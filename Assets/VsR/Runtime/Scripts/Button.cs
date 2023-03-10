using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace VsR {
	public class Button : MonoBehaviour {
		public event System.Action onPressed;

		[SerializeField] private Transform m_visualPress;
		[SerializeField] private AudioClip m_pressSound;
		[SerializeField] private float m_pressOffset = 0.1f;
		private Vector3 m_initVisualPressPosition;
		private bool m_pressed = false;
		private List<Collider> m_pressingColliders = new List<Collider>();

		protected void Awake() {
			m_initVisualPressPosition = m_visualPress.localPosition;
		}

		private void OnTriggerEnter(Collider collider) {
			if (!m_pressingColliders.Contains(collider))
				m_pressingColliders.Add(collider);

			if (m_pressed)
				return;

			m_pressed = true;
			m_visualPress.localPosition = m_initVisualPressPosition - m_visualPress.up * m_pressOffset;
			SoundPoolManager.Instance.PlaySound(m_pressSound, transform.position, Random.Range(0.9f, 1.1f));
			onPressed?.Invoke();
		}

		private void OnTriggerExit(Collider collider) {
			m_pressingColliders.Remove(collider);

			if (m_pressingColliders.Count != 0)
				return;

			m_pressed = false;
			m_visualPress.localPosition = m_initVisualPressPosition;
		}
	}
}

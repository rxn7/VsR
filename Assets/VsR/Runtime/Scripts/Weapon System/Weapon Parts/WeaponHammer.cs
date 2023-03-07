using UnityEngine;
using System.Collections;

namespace VsR {
	public class WeaponHammer : WeaponPart {
		[SerializeField] private Vector3 m_maxRotation;
		private Vector3 m_initRotation;

		private void Start() {
			m_initRotation = transform.localEulerAngles;
			m_weapon.onFire += OnFire;
		}

		private void OnFire() {
			StartCoroutine(HammerStrike());
		}

		private IEnumerator HammerStrike() {
			float duration = 25 * 0.001f;
			float elapsed = 0.0f;
			while (elapsed < duration) {
				transform.localEulerAngles = Vector3.Lerp(m_initRotation, m_maxRotation, elapsed / duration);
				elapsed += Time.deltaTime;
				yield return null;
			}
			elapsed = 0;
			while (elapsed < duration) {
				transform.localEulerAngles = Vector3.Lerp(m_maxRotation, m_initRotation, elapsed / duration);
				elapsed += Time.deltaTime;
				yield return null;
			}
		}
	}
}
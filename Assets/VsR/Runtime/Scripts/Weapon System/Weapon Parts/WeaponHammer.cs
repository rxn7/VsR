using UnityEngine;
using System.Collections;

namespace VsR {
	public class WeaponHammer : MonoBehaviour, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		[SerializeField] private Vector3 m_maxRotation;
		private Vector3 m_initRotation;

		private void Start() {
			m_initRotation = transform.localEulerAngles;
			Weapon.onFire += OnFire;
		}

		private void OnFire() {
			StopAllCoroutines();
			StartCoroutine(HammerStrike());
		}

		private IEnumerator HammerStrike() {
			float duration = 25 * 0.001f;
			float elapsed = 0.0f;
			while (elapsed < duration) {
				elapsed += Time.deltaTime;
				transform.localEulerAngles = Vector3.Lerp(m_initRotation, m_maxRotation, elapsed / duration);
				yield return null;
			}
			elapsed = 0;
			while (elapsed < duration) {
				elapsed += Time.deltaTime;
				transform.localEulerAngles = Vector3.Lerp(m_maxRotation, m_initRotation, elapsed / duration);
				yield return null;
			}
		}
	}
}
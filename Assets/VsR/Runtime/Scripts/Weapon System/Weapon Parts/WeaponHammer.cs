using UnityEngine;
using System.Collections;

namespace VsR {
	public class WeaponHammer : MonoBehaviour, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		[SerializeField] private Vector3 m_maxRotation;
		private Quaternion m_initRotation;

		private void Awake() {
			IWeaponPart.Validate(this);
		}

		private void Start() {
			m_initRotation = transform.localRotation;
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
				transform.localRotation = Quaternion.Lerp(m_initRotation, Quaternion.Euler(m_maxRotation), elapsed / duration);
				yield return null;
			}
			elapsed = 0;
			while (elapsed < duration) {
				elapsed += Time.deltaTime;
				transform.localRotation = Quaternion.Lerp(Quaternion.Euler(m_maxRotation), m_initRotation, elapsed / duration);
				yield return null;
			}
		}
	}
}
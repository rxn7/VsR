using System.Collections;
using UnityEngine;

namespace VsR {
	public class PistolSlide : WeaponSlide {
		[SerializeField] private float m_shootSlideBackAnimationDuration;
		[SerializeField] private Transform m_slideLockTransform;

		protected override void Start() {
			base.Start();
			weapon.onFire += OnWeaponFire;
		}

		private IEnumerator SlideBackAnimation() {
			float elapsed = 0.0f;
			while (elapsed < m_shootSlideBackAnimationDuration) {
				transform.localPosition = Vector3.Lerp(transform.localPosition, m_maxSlidePosition, elapsed / m_shootSlideBackAnimationDuration);
				elapsed += Time.deltaTime;
				yield return null;
			}
		}

		private IEnumerator ShootSequence() {
			StartCoroutine(SlideBackAnimation());

			yield return new WaitForSeconds(m_shootSlideBackAnimationDuration);
			weapon.EjectCartridge(false);

			if (!weapon.CartridgeInChamber) {
				Locked = true;
				transform.localPosition = m_maxSlidePosition;
			} else {
				StartCoroutine(ReleaseAnimation());
			}
		}

		private void OnWeaponFire() {
			StartCoroutine(ShootSequence());
		}
	}
}

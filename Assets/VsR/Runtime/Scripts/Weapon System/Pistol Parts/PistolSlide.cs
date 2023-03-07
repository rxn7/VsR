using System.Collections;
using UnityEngine;

namespace VsR {
	public class PistolSlide : WeaponSlide {
		[SerializeField] private float m_shootSlideBackAnimationDuration;
		[SerializeField] protected Vector3 m_lockedSlidePosition;

		public delegate void LockEvent();
		public event LockEvent onLocked;
		public event LockEvent onUnlocked;

		private bool _m_locked = false;
		public bool Locked {
			get => _m_locked;
			set {
				_m_locked = value;
				if (_m_locked)
					onLocked?.Invoke();
				else
					onUnlocked?.Invoke();
			}
		}

		protected override bool CanRelease => base.CanRelease && !Locked;

		protected virtual void Start() {
			m_weapon.onFire += OnWeaponFire;
			onLocked += () => transform.localPosition = m_lockedSlidePosition;
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
			m_weapon.EjectCartridge(false);

			if (!m_weapon.CartridgeInChamber) {
				Locked = true;
			} else {
				StartCoroutine(ReleaseAnimation());
			}
		}

		private void OnWeaponFire() {
			StartCoroutine(ShootSequence());
		}
	}
}

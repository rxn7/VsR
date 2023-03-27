using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class PistolSlide : WeaponSlide {
		private float m_shootAnimationDuration;

		public event System.Action onLocked;
		public event System.Action onUnlocked;

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

		protected override void Awake() {
			base.Awake();
			m_shootAnimationDuration = Weapon.Data.SecondsPerRound;
			Weapon.onFire += OnWeaponFire;
			onLocked += () => transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, 0.95f);
		}

		private IEnumerator ShootSequence() {
			float segmentDuration = m_shootAnimationDuration * 0.5f;

			float elapsed = 0.0f;
			while (elapsed < segmentDuration) {
				elapsed += Time.deltaTime;
				transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, elapsed / segmentDuration);
				yield return null;
			}

			Weapon.EjectCartridge(false);

			if (!Weapon.CartridgeInChamber) {
				Locked = true;
			} else {
				elapsed = 0.0f;
				while (elapsed < segmentDuration) {
					elapsed += Time.deltaTime;
					transform.localPosition = Vector3.Lerp(m_maxSlidePosition, m_initPosition, elapsed / segmentDuration);
					yield return null;
				}
			}
		}

		private void OnWeaponFire() {
			StopAllCoroutines();
			StartCoroutine(ShootSequence());
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (Locked)
				return false;

			return base.IsSelectableBy(interactor);
		}
	}
}

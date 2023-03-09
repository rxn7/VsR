using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class PistolSlide : WeaponSlide {
		[SerializeField] private float m_shootAnimationDuration;
		[SerializeField] protected Vector3 m_lockedSlidePosition;

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

		protected virtual void Start() {
			Weapon.onFire += OnWeaponFire;
			onLocked += () => transform.localPosition = m_lockedSlidePosition;
		}

		private IEnumerator ShootSequence() {
			float segmentDuration = m_shootAnimationDuration * 0.5f;

			float elapsed = 0.0f;
			while (elapsed < segmentDuration) {
				transform.localPosition = Vector3.Lerp(m_initPosition, m_maxSlidePosition, elapsed / segmentDuration);
				elapsed += Time.deltaTime;
				yield return null;
			}

			Weapon.EjectCartridge(false);

			if (!Weapon.CartridgeInChamber) {
				Locked = true;
			} else {
				elapsed = 0.0f;
				while (elapsed < segmentDuration) {
					transform.localPosition = Vector3.Lerp(m_maxSlidePosition, m_initPosition, elapsed / segmentDuration);
					elapsed += Time.deltaTime;
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

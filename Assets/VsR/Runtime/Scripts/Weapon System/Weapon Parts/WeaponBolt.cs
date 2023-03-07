using UnityEngine;

namespace VsR {
	public class WeaponBolt : MonoBehaviour {
		[SerializeField] protected WeaponBase m_weapon;
		[SerializeField] protected Vector3 m_maxPosition;
		protected float m_shootAnimationDuration;

		protected Transform m_parent;
		protected Vector3 m_initPosition;
		protected bool m_boltFireAnimating = false;
		protected bool m_boltRoundEjected = false;
		protected float m_boltAlpha = 0.0f;
		private bool _m_isOpen = false;

		public bool IsOpen {
			get => _m_isOpen;
			set {
				_m_isOpen = value;
				transform.SetParent(value ? m_weapon.transform : m_parent, false);
			}
		}

		protected void Awake() {
			m_initPosition = transform.localPosition;
			m_parent = transform.parent;
			m_weapon.onFire += OnFire;
			m_shootAnimationDuration = m_weapon.Data.SecondsPerRound;
		}

		private void OnFire() {
			if (!m_weapon.CartridgeInChamber) {
				IsOpen = true;
				return;
			}

			m_boltFireAnimating = true;
		}

		private void Update() {
			if (m_boltFireAnimating) {
				m_boltAlpha += Time.deltaTime / m_shootAnimationDuration;

				float alpha = Mathf.Sin(m_boltAlpha * Mathf.PI);
				transform.localPosition = Vector3.Lerp(m_initPosition, m_maxPosition, alpha);

				if (m_boltAlpha > 0.15f && !m_boltRoundEjected) {
					m_boltRoundEjected = true;
					m_weapon.EjectCartridge(false);
				}

				if (m_boltAlpha > 1.0f) {
					m_boltRoundEjected = false;
					m_boltFireAnimating = false;
					m_boltAlpha = 0.0f;
				}
			} else {
				transform.localPosition = IsOpen ? m_maxPosition : m_initPosition;
			}
		}
	}
}
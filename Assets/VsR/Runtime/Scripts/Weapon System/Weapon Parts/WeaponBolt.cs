using System.Collections;
using UnityEngine;

namespace VsR {
	public class WeaponBolt : MonoBehaviour, IWeaponPart {
		public event System.Action onOpen;
		public event System.Action onRelease;
		public bool IsAnimating { get; set; } = false;

		[field: SerializeField] public Weapon Weapon { get; set; }
		[SerializeField] protected Vector3 m_maxPosition;

		private Transform m_parent;
		private Vector3 m_initPosition;
		private bool _m_isOpen = false;

		public bool IsOpen {
			get => _m_isOpen;
			set {
				_m_isOpen = value;

                if(value)
                    onOpen?.Invoke();
                else 
                    onRelease?.Invoke();

				transform.SetParent(value ? Weapon.transform : m_parent, false);
			}
		}

		protected void Awake() {
			m_initPosition = transform.localPosition;
			m_parent = transform.parent;

			Weapon.onFire += OnFire;
		}

		private void OnFire() {
            StopAllCoroutines();
            StartCoroutine(FireAnimationCoroutine());
		}

        public void SetPullPercentage(float percentage) {
            transform.localPosition = Vector3.Lerp(m_initPosition, m_maxPosition, percentage);
        }

        private IEnumerator FireAnimationCoroutine() {
            IsAnimating = true;
            
            float elapsed = 0.0f;
            bool cartridgeEjected = false;
            bool racked = false;
            while(elapsed < Weapon.Data.SecondsPerRound) {
                elapsed += Time.deltaTime;
                float elapsedRatio = elapsed / Weapon.Data.SecondsPerRound;

                float alpha = Mathf.Sin(elapsedRatio * Mathf.PI);
                transform.localPosition = Vector3.Lerp(m_initPosition, m_maxPosition, alpha);

                if (elapsedRatio > 0.15f && !cartridgeEjected) {
                    cartridgeEjected = true;
                    Weapon.EjectChamberedCartridge(false);
                } else if(elapsedRatio > 0.65f && !racked) {
                    racked = true;
                    Weapon.Rack();
                }

                yield return null;
            }

            IsAnimating = false;
        }
	}
}
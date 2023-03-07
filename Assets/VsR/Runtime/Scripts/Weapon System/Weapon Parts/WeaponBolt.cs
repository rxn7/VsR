using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

namespace VsR {
	public class WeaponBolt : MonoBehaviour {
		[SerializeField] protected WeaponBase m_weapon;
		[SerializeField] protected Vector3 m_maxSlidePosition;
		[SerializeField] protected float m_releaseAnimationSpeed = 0.03f;

		protected Vector3 m_initPosition;
		protected Vector3 m_startHandLocalPosition;
		protected float m_initToMaxSlideDistance;
		protected bool _m_boltOpen = false;
		protected Hand m_hand;

		public bool Open {
			get => _m_boltOpen;
			set {
				_m_boltOpen = value;
				transform.localPosition = value ? m_maxSlidePosition : m_initPosition;
			}
		}

		protected void Awake() {
			m_initPosition = transform.localPosition;
			m_initToMaxSlideDistance = Vector3.Distance(m_initPosition, m_maxSlidePosition);
			m_weapon.onFire += OnFire;
		}

		private void OnFire() {
			if (!m_weapon.CartridgeInChamber) {
				_m_boltOpen = true;
			}
		}

		protected virtual void UpdateSlideMovement() {
		}

		protected void Rack() {
			SoundPoolManager.Instance.PlaySound(m_weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_weapon.TryToCock();
			_m_boltOpen = true;
		}

		protected void RackBack() {
			SoundPoolManager.Instance.PlaySound(m_weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
			_m_boltOpen = false;
		}
	}
}
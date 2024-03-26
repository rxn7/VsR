using UnityEngine;

namespace VsR {
	public class PistolSlideLock : MonoBehaviour {
		[SerializeField] private WeaponSlide m_slide;
		[SerializeField] private Vector3 m_lockedPosition;
		[SerializeField] private Vector3 m_lockedRotation;

		private Vector3 m_initPosition, m_initRotation;

		private void Awake() {
			m_initPosition = transform.localPosition;
			m_initRotation = transform.localEulerAngles;

			m_slide.Bolt.onOpen += Lock;
			m_slide.Bolt.onRelease += Unlock;
		}

		private void Lock() {
			transform.localPosition = m_lockedPosition;
			transform.localEulerAngles = m_lockedRotation;
		}

		private void Unlock() {
			transform.localPosition = m_initPosition;
			transform.localEulerAngles = m_initRotation;
		}
	}
}

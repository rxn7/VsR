using UnityEngine;

namespace VsR {
	public class Recoil : MonoBehaviour {
		private float m_returnSpeed = 20;
		private float m_snapiness = 12;

		private RecoilInfo? m_info;
		private Vector3 m_initPosition;
		private Quaternion m_initRotation;
		private Vector3 m_targetPosition;
		private Quaternion m_targetRotation;

		private void Awake() {
			m_initPosition = transform.localPosition;
			m_initRotation = transform.localRotation;
		}

		private void Update() {
			if (!m_info.HasValue)
				return;

			m_targetRotation = Quaternion.Slerp(m_targetRotation, m_initRotation, Time.deltaTime * m_returnSpeed);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, m_targetRotation, Time.deltaTime * m_snapiness);

			m_targetPosition = Vector3.Lerp(m_targetPosition, m_initPosition, Time.deltaTime * m_returnSpeed);
			transform.localPosition = Vector3.Lerp(transform.localPosition, m_targetPosition, Time.deltaTime * m_snapiness);
		}

		public void AddRecoil(RecoilInfo info) {
			m_info = info;

			// TODO: Randomize
			m_targetPosition += info.linearForce;
			m_targetRotation *= Quaternion.Euler(info.angularForce);
		}
	}
}
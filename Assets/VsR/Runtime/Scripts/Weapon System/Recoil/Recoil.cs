using UnityEngine;
using VsR.Math;

namespace VsR {
	public class Recoil : MonoBehaviour {
		private float m_returnSpeed = 20;
		private float m_snapiness = 12;

		private RecoilInfo? m_info;
		private Vector3 m_initPosition;
		private Quaternion m_initRotation;
		private Vector3 m_targetPosition;
		private Quaternion m_targetRotation = Quaternion.identity;

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

			m_targetPosition += info.force + VectorHelper.RandomVector(
				new Math.FloatRange(-info.forceRandomness.x, info.forceRandomness.x),
				new Math.FloatRange(-info.forceRandomness.y, info.forceRandomness.y),
				new Math.FloatRange(-info.forceRandomness.z, info.forceRandomness.z)
			);

			m_targetRotation *= Quaternion.Euler(info.torque + VectorHelper.RandomVector(
				new Math.FloatRange(-info.torqueRandomness.x, info.torqueRandomness.x),
				new Math.FloatRange(-info.torqueRandomness.y, info.torqueRandomness.y),
				new Math.FloatRange(-info.torqueRandomness.z, info.torqueRandomness.z)
			));
		}
	}
}
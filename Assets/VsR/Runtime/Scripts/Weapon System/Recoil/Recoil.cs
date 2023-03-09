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

			m_targetRotation = Quaternion.Lerp(m_targetRotation, m_initRotation, Time.deltaTime * m_returnSpeed);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, m_targetRotation, Time.deltaTime * m_snapiness);

			m_targetPosition = Vector3.Lerp(m_targetPosition, m_initPosition, Time.deltaTime * m_returnSpeed);
			transform.localPosition = Vector3.Lerp(transform.localPosition, m_targetPosition, Time.deltaTime * m_snapiness);
		}

		public void AddRecoil(Weapon weapon) {
			m_info = weapon.Data.recoilInfo;

			bool isGuardHoldHeld = weapon.GuardHand != null;

			float forceMultiplier = isGuardHoldHeld ? 0.25f : 1.0f;
			float torqueMultiplier = isGuardHoldHeld ? 0.3f : 1.0f;

			m_targetPosition += (weapon.Data.recoilInfo.force + VectorHelper.RandomVector(
				new Math.FloatRange(-weapon.Data.recoilInfo.forceRandomness.x, weapon.Data.recoilInfo.forceRandomness.x),
				new Math.FloatRange(-weapon.Data.recoilInfo.forceRandomness.y, weapon.Data.recoilInfo.forceRandomness.y),
				new Math.FloatRange(-weapon.Data.recoilInfo.forceRandomness.z, weapon.Data.recoilInfo.forceRandomness.z)
			)) * forceMultiplier;

			m_targetRotation *= Quaternion.Euler((weapon.Data.recoilInfo.torque + VectorHelper.RandomVector(
				new Math.FloatRange(-weapon.Data.recoilInfo.torqueRandomness.x, weapon.Data.recoilInfo.torqueRandomness.x),
				new Math.FloatRange(-weapon.Data.recoilInfo.torqueRandomness.y, weapon.Data.recoilInfo.torqueRandomness.y),
				new Math.FloatRange(-weapon.Data.recoilInfo.torqueRandomness.z, weapon.Data.recoilInfo.torqueRandomness.z)
			)) * torqueMultiplier);
		}
	}
}
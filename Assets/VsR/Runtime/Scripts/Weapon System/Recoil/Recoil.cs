using UnityEngine;

namespace VsR {
	public class Recoil : MonoBehaviour {
		private float m_returnSpeed = 20;
		private float m_snapiness = 12;

		private Vector3 m_initPosition;
		private Quaternion m_initRotation;
		private Vector3 m_targetPosition;
		private Quaternion m_targetRotation = Quaternion.identity;

		private void Awake() {
			m_initPosition = transform.localPosition;
			m_initRotation = transform.localRotation;
		}

		private void Update() {
			m_targetRotation = Quaternion.Lerp(m_targetRotation, m_initRotation, Time.deltaTime * m_returnSpeed);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, m_targetRotation, Time.deltaTime * m_snapiness);

			m_targetPosition = Vector3.Lerp(m_targetPosition, m_initPosition, Time.deltaTime * m_returnSpeed);
			transform.localPosition = Vector3.Lerp(transform.localPosition, m_targetPosition, Time.deltaTime * m_snapiness);
		}

		public void AddRecoil(Weapon weapon) {
			if (weapon.GuardHand)
				AddRecoilDoubleHand(weapon);
			else
				AddRecoilSingleHand(weapon.Data);
		}

		private void AddRecoilSingleHand(WeaponData data) {
			m_targetPosition += data.recoilInfo.force.RandomValue();
			m_targetRotation *= Quaternion.Euler(data.recoilInfo.torque.RandomValue());
		}

		private void AddRecoilDoubleHand(Weapon weapon) {
			float distanceToHold = Vector3.Distance(weapon.GuardHand.attachTransform.position, weapon.HeldGuardHold.transform.position);

			m_targetPosition += Vector3.forward * weapon.Data.recoilInfo.force.z.RandomValue();

			// TODO: Guard hold recoil
		}
	}
}
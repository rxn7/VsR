using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VsR.Math;

namespace VsR {
	public class WeaponSlide : XRBaseInteractable {
		[HideInInspector] public Weapon weapon;
		[SerializeField] private Vector3 m_slideEnd;
		private Vector3 m_slideStart;

		private float m_slideDistance;
		private float m_startHandLocalY;
		private FloatRange m_slideRange;

		private bool m_cocked = false;
		private Hand m_hand;

		protected override void Awake() {
			base.Awake();
			m_slideStart = transform.localPosition;
			m_slideDistance = Vector3.Distance(m_slideStart, m_slideEnd);

			m_slideRange.min = Mathf.Min(m_slideStart.y, m_slideEnd.y);
			m_slideRange.max = Mathf.Max(m_slideStart.y, m_slideEnd.y);
		}

		private void UpdateSlideMovement() {
			float handLocalY = transform.InverseTransformPoint(m_hand.attachTransform.position).y;
			float handDiff = Mathf.Abs(handLocalY - m_startHandLocalY);
			Vector3 localPosition = transform.localPosition;

			if (m_slideRange.min == m_slideStart.y)
				handDiff *= -1;

			localPosition.y = Mathf.Clamp(m_slideStart.y + handDiff, m_slideRange.min, m_slideRange.max);
			transform.localPosition = localPosition;

			float slidePercentage = (m_slideDistance - Vector3.Distance(m_slideEnd, localPosition)) / m_slideDistance;

			if (m_cocked && slidePercentage < 0.6f) {
				SoundManager.Instance.PlaySound(weapon.Data.cockBackSound, transform.position, Random.Range(0.9f, 1.1f));
				m_cocked = false;
			} else if (!m_cocked && slidePercentage >= 0.99f) {
				SoundManager.Instance.PlaySound(weapon.Data.cockSound, transform.position, Random.Range(0.9f, 1.1f));
				weapon.TryToCock();
				m_cocked = true;
			}
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (!weapon.isSelected)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (Hand)args.interactorObject;

			// If slide is stopped, use end slide position as starting hand local position
			if (weapon.Data.weaponType == WeaponData.WeaponType.Pistol && weapon.Animator.GetBool("SlideStop"))
				m_startHandLocalY = m_slideEnd.y;
			else
				m_startHandLocalY = transform.InverseTransformPoint(m_hand.attachTransform.position).y;
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);
			m_hand = null;

			Vector3 localPosition = transform.localPosition;
			if (weapon.Data.weaponType == WeaponData.WeaponType.Pistol && weapon.Animator.GetBool("SlideStop"))
				localPosition.y = m_slideEnd.y;
			else
				localPosition.y = m_slideStart.y;
			transform.localPosition = localPosition;

			if (m_cocked) {
				SoundManager.Instance.PlaySound(weapon.Data.cockBackSound, transform.position, Random.Range(0.9f, 1.1f));

				if (weapon.Data.weaponType == WeaponData.WeaponType.Pistol)
					weapon.Animator.SetBool("SlideStop", false);
			}

			m_cocked = false;
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (m_hand != null && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Late)
				UpdateSlideMovement();
		}
	}
}
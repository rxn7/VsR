using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponSlide : XRBaseInteractable {
		[SerializeField] private Weapon m_weapon;
		[SerializeField] private Vector3 m_slideStart, m_slideEnd;

		private float m_startHandLocalY;

		private bool m_cocked = false;
		private Hand m_hand;

		protected override void Awake() {
			base.Awake();

			if (m_weapon == null) {
				Destroy(gameObject);
				throw new UnassignedReferenceException($"m_weapon is not assigned!");
			}
		}

		private void UpdateSlideMovement() {
			float handLocalY = transform.InverseTransformPoint(m_hand.attachTransform.position).y;
			float handDiff = m_startHandLocalY - handLocalY;

			Vector3 localPosition = transform.localPosition;
			localPosition.y = Mathf.Clamp(m_slideStart.y - handDiff, m_slideStart.y, m_slideEnd.y);
			transform.localPosition = localPosition;

			float slidePercentage = (m_slideStart.y - localPosition.y) / (m_slideStart.y - m_slideEnd.y);

			if (m_cocked && slidePercentage < 0.6f) {
				SoundManager.Instance.PlaySound(m_weapon.Data.cockBackSound, transform.position, Random.Range(0.9f, 1.1f));
				m_cocked = false;
			}

			if (!m_cocked && slidePercentage >= 0.99f) {
				SoundManager.Instance.PlaySound(m_weapon.Data.cockSound, transform.position, Random.Range(0.9f, 1.1f));
				m_weapon.Cock();
				m_cocked = true;
			}
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (!m_weapon.isSelected)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			m_hand = (Hand)args.interactorObject;

			// If slide is stopped, use end slide position as starting hand local position
			if (m_weapon.Animator.GetBool("SlideStop"))
				m_startHandLocalY = m_slideEnd.y;
			else
				m_startHandLocalY = transform.InverseTransformPoint(m_hand.attachTransform.position).y;
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);
			m_hand = null;

			Vector3 localPosition = transform.localPosition;
			localPosition.y = m_weapon.Animator.GetBool("SlideStop") ? m_slideEnd.y : m_slideStart.y;
			transform.localPosition = localPosition;

			if (m_cocked) {
				SoundManager.Instance.PlaySound(m_weapon.Data.cockBackSound, transform.position, Random.Range(0.9f, 1.1f));
				m_weapon.Animator.SetBool("SlideStop", false);
			}

			m_cocked = false;
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (m_hand != null && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Late) {
				UpdateSlideMovement();
			}
		}
	}
}
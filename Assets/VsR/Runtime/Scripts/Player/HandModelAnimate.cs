using UnityEngine;

namespace VsR {
	public class HandModelAnimate : MonoBehaviour {
		[SerializeField] private Hand m_hand;

		private void Update() {
			float triggerValue = m_hand.TriggerAction.ReadValue<float>();
			float gripValue = m_hand.GripAction.ReadValue<float>();

			m_hand.Animator.SetFloat("Trigger", triggerValue);
			m_hand.Animator.SetFloat("Grip", gripValue);
		}
	}

}
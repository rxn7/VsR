using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public enum HandType : byte { Left = 0, Right }

	public class Hand : XRDirectInteractor {
		[Space]
		[Space]
		[Header("Hand")]
		[SerializeField] private HandType m_handType;
		[SerializeField] Animator m_animator;

		public Animator Animator => m_animator;
		public HandType HandType => m_handType;

		public InputAction TriggerAction { get; private set; }
		public InputAction GripAction { get; private set; }
		public InputAction MagReleaseAction { get; private set; }
		public InputAction SlideReleaseAction { get; private set; }

		protected override void Start() {
			base.Start();
			InitInputActions();
		}

		private void InitInputActions() {
			TriggerAction = InputActionManager.Instance.GetInteractionAction(m_handType, "Activate Value");
			GripAction = InputActionManager.Instance.GetInteractionAction(m_handType, "Select Value");
			MagReleaseAction = InputActionManager.Instance.GetInteractionAction(m_handType, "Mag Release");
			SlideReleaseAction = InputActionManager.Instance.GetInteractionAction(m_handType, "Slide Release");
		}

		public void ApplyHapticFeedback(HapticFeedback feedback) {
			xrController.SendHapticImpulse(feedback.intensity, feedback.duration);
		}
	}
}
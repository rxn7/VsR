using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public enum HandType : byte { Left = 0, Right }

	public class Hand : XRDirectInteractor {
		[Header("Hand")]
		[SerializeField] private HandType m_handType;
		[SerializeField] private Recoil m_recoil;

		public InputAction TriggerAction { get; private set; }
		public InputAction GrabAction { get; private set; }
		public InputAction MagReleaseAction { get; private set; }
		public InputAction SlideReleaseAction { get; private set; }

		public HandType HandType => m_handType;
		public Recoil Recoil => m_recoil;
		public bool IsGrabbing => interactablesSelected.Count > 0;

		protected override void Awake() {
			base.Awake();
		}

		protected override void Start() {
			base.Start();
			InitInputActions();
		}

		private void InitInputActions() {
			TriggerAction = InputActionManager.GetInteractionAction(m_handType, "Activate Value");
			GrabAction = InputActionManager.GetInteractionAction(m_handType, "Select Value");
			MagReleaseAction = InputActionManager.GetInteractionAction(m_handType, "Mag Release");
			SlideReleaseAction = InputActionManager.GetInteractionAction(m_handType, "Slide Release");
		}

		public void ApplyHapticFeedback(HapticFeedback feedback) {
			SendHapticImpulse(feedback.intensity, feedback.duration);
		}
	}
}
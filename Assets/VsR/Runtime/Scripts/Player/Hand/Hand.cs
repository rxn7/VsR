using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public enum HandType : byte { Left = 0, Right }

	[RequireComponent(typeof(ActionBasedController))]
	[RequireComponent(typeof(XRInteractionGroup))]
	public class Hand : MonoBehaviour {
		[Header("Hand")]
		[SerializeField] private HandType m_handType;
		[SerializeField] private Recoil m_recoil;
		private ActionBasedController m_controller;
		private XRInteractionGroup m_group;

		public InputAction TriggerAction { get; private set; }
		public InputAction GrabAction { get; private set; }
		public InputAction MagReleaseAction { get; private set; }
		public InputAction SlideReleaseAction { get; private set; }

		public XRBaseInteractor Interactor => m_group.activeInteractor as XRBaseInteractor;
		public bool IsGrabbing => (m_group.activeInteractor as XRBaseInteractor)?.interactablesSelected.Count > 0;
		public HandType HandType => m_handType;
		public Recoil Recoil => m_recoil;

		private void Awake() {
			m_controller = GetComponent<ActionBasedController>();
			m_group = GetComponent<XRInteractionGroup>();
		}

		private void Start() {
			InitInputActions();
		}

		private void InitInputActions() {
			TriggerAction = InputActionManager.GetInteractionAction(m_handType, "Activate Value");
			GrabAction = InputActionManager.GetInteractionAction(m_handType, "Select Value");
			MagReleaseAction = InputActionManager.GetInteractionAction(m_handType, "Mag Release");
			SlideReleaseAction = InputActionManager.GetInteractionAction(m_handType, "Slide Release");
		}

		public void ApplyHapticFeedback(HapticFeedback feedback) {
			m_controller.SendHapticImpulse(feedback.intensity, feedback.duration);
		}
	}
}
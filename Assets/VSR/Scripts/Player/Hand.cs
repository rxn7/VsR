using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class Hand : XRDirectInteractor {
		public enum HandType : byte { Left, Right }

		[Space]
		[Header("Weapon")]
		[SerializeField] private HandType m_handType;
		[SerializeField] Animator m_animator;

		public Animator Animator => m_animator;
		public HandType Type => m_handType;

		public InputAction TriggerAction { get; private set; }
		public InputAction GripAction { get; private set; }
		public InputAction MagReleaseAction { get; private set; }
		public InputAction SlideReleaseAction { get; private set; }

		protected override void Awake() {
			base.Awake();
			InitInputActions();
		}

		private void InitInputActions() {
			InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>($"XRI Input Actions");
			InputActionMap inputActionMap = inputActionAsset?.FindActionMap($"XRI {(m_handType == Hand.HandType.Left ? "Left" : "Right")}Hand Interaction", true);

			TriggerAction = inputActionMap["Activate Value"];
			GripAction = inputActionMap["Select Value"];
			MagReleaseAction = inputActionMap["Mag Release"];
			SlideReleaseAction = inputActionMap["Slide Release"];
		}
	}
}
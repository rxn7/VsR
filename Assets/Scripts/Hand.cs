using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Hand : XRDirectInteractor {
	public enum HandType : byte { Left, Right }

	[Header("Weapon")]
	[SerializeField] private HandType m_handType;
	[SerializeField] Animator m_animator;

	public Animator Animator => m_animator;
	public HandType Type => m_handType;

	public InputAction TriggerAction { get; private set; }
	public InputAction GripAction { get; private set; }

	protected override void Awake() {
		base.Awake();
		InitInputActions();
	}

	private void InitInputActions() {
		InputActionAsset inputActionAsset = Resources.Load<InputActionAsset>($"XRI Default Input Actions");
		InputActionMap inputActionMap = inputActionAsset?.FindActionMap($"XRI {(m_handType == Hand.HandType.Left ? "Left" : "Right")}Hand Interaction", true);

		TriggerAction = inputActionMap?["Activate Value"];
		GripAction = inputActionMap?["Select Value"];
	}
}
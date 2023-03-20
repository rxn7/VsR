using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	public static class InputActionManager {
		private static InputActionAsset s_inputActionAsset;

		public static InputActionMap HeadMap { get; private set; }
		public static InputActionMap UiMap { get; private set; }

		private static InputActionMap[] s_handLocomationMaps = new InputActionMap[2];
		private static InputActionMap[] s_handInteractionMaps = new InputActionMap[2];
		private static InputActionMap[] s_handGenericMaps = new InputActionMap[2];

		public static InputAction GetGenericAction(HandType handType, string name) => s_handGenericMaps[(int)handType][name];
		public static InputAction GetInteractionAction(HandType handType, string name) => s_handInteractionMaps[(int)handType][name];
		public static InputAction GetLocomotionAction(HandType handType, string name) => s_handLocomationMaps[(int)handType][name];

		[RuntimeInitializeOnLoadMethod]
		private static void Init() {
			s_inputActionAsset = Resources.Load<InputActionAsset>("XRI Input Actions");

			HeadMap = s_inputActionAsset.FindActionMap("XRI Head");
			UiMap = s_inputActionAsset.FindActionMap("XRI UI");
			LoadHandMaps(HandType.Left);
			LoadHandMaps(HandType.Right);
		}

		private static void LoadHandMaps(HandType handType) {
			int idx = (int)handType;
			string handName = Enum.GetName(typeof(HandType), handType);
			s_handGenericMaps[idx] = s_inputActionAsset.FindActionMap($"XRI {handName}Hand");
			s_handInteractionMaps[idx] = s_inputActionAsset.FindActionMap($"XRI {handName}Hand Interaction");
			s_handLocomationMaps[idx] = s_inputActionAsset.FindActionMap($"XRI {handName}Hand Locomation");
		}
	}
}
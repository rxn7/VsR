using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	public class InputActionManager : SingletonBehaviour<InputActionManager> {
		private InputActionAsset m_inputActionAsset;

		public InputActionMap HeadMap { get; private set; }
		public InputActionMap UiMap { get; private set; }

		private InputActionMap[] m_handLocomationMaps = new InputActionMap[2];
		private InputActionMap[] m_handInteractionMaps = new InputActionMap[2];
		private InputActionMap[] m_handGenericMaps = new InputActionMap[2];

		public InputAction GetGenericAction(HandType handType, string name) => m_handGenericMaps[(int)handType][name];
		public InputAction GetInteractionAction(HandType handType, string name) => m_handInteractionMaps[(int)handType][name];
		public InputAction GetLocomotionAction(HandType handType, string name) => m_handLocomationMaps[(int)handType][name];

		protected override void Awake() {
			base.Awake();

			m_inputActionAsset = Resources.Load<InputActionAsset>("XRI Input Actions");

			HeadMap = m_inputActionAsset.FindActionMap("XRI Head");
			UiMap = m_inputActionAsset.FindActionMap("XRI UI");
			LoadHandMaps(HandType.Left);
			LoadHandMaps(HandType.Right);
		}

		private void LoadHandMaps(HandType handType) {
			int idx = (int)handType;
			string handName = Enum.GetName(typeof(HandType), handType);
			m_handGenericMaps[idx] = m_inputActionAsset.FindActionMap($"XRI {handName}Hand");
			m_handInteractionMaps[idx] = m_inputActionAsset.FindActionMap($"XRI {handName}Hand Interaction");
			m_handLocomationMaps[idx] = m_inputActionAsset.FindActionMap($"XRI {handName}Hand Locomation");
		}
	}
}
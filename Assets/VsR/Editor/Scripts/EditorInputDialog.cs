using System;
using UnityEditor;
using UnityEngine;

namespace VsR.Editors {
	public class EditorInputDialog : EditorWindow {
		private string m_description;
		private string m_inputText;
		private bool m_initializedPosition = false;
		private Action m_onConfirm;
		private bool m_shouldClose = false;

		void OnGUI() {
			var e = Event.current;
			if (e.type == EventType.KeyDown) {
				switch (e.keyCode) {
					case KeyCode.Escape:
						m_shouldClose = true;
						break;

					case KeyCode.Return:
					case KeyCode.KeypadEnter:
						m_onConfirm?.Invoke();
						m_shouldClose = true;
						break;
				}
			}

			if (m_shouldClose) {
				Close();
				return;
			}

			Rect root = EditorGUILayout.BeginVertical();

			EditorGUILayout.Space(12);
			EditorGUILayout.LabelField(m_description);

			EditorGUILayout.Space(8);
			GUI.SetNextControlName("inText");
			m_inputText = EditorGUILayout.TextField("", m_inputText);
			GUI.FocusControl("inText");
			EditorGUILayout.Space(12);

			Rect r = EditorGUILayout.GetControlRect();
			r.width /= 2;
			if (GUI.Button(r, "OK")) {
				m_onConfirm?.Invoke();
				m_shouldClose = true;
			}

			r.x += r.width;
			if (GUI.Button(r, "Cancel")) {
				m_inputText = null;
				m_shouldClose = true;
			}

			EditorGUILayout.Space(8);
			EditorGUILayout.EndVertical();

			if (root.width != 0 && minSize != root.size)
				minSize = maxSize = root.size;

			if (!m_initializedPosition) {
				Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
				position = new Rect(mousePos.x + 32, mousePos.y, position.width, position.height);
				m_initializedPosition = true;
			}
		}

		public static string Show(string title, string description, string inputText) {
			string input = null;

			var window = CreateInstance<EditorInputDialog>();
			window.titleContent = new GUIContent(title);
			window.m_description = description;
			window.m_inputText = inputText;
			window.m_onConfirm += () => input = window.m_inputText;
			window.ShowModal();

			return input;
		}
	}
}
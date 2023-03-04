using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	[CustomEditor(typeof(Weapon), true)]
	public class WeaponEditor : Editor {
		SerializedProperty[] m_properties;

		private void OnEnable() {
			m_properties = new SerializedProperty[] {
				serializedObject.FindProperty("m_data"),
				serializedObject.FindProperty("m_magSlot"),
				serializedObject.FindProperty("m_slide"),
				serializedObject.FindProperty("m_visualTriggerTransform"),
				serializedObject.FindProperty("m_barrelEndPoint"),
				serializedObject.FindProperty("m_cartridgeEjectPoint"),
				serializedObject.FindProperty("m_cartridgeInChamber"),
			};
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			bool anyNull = false;

			foreach (SerializedProperty prop in m_properties) {
				if (!prop.objectReferenceValue) {
					EditorGUIUtility.DrawColorSwatch(new Rect(), Color.red);
					anyNull = true;
				}

				EditorGUILayout.PropertyField(prop);
			}

			if (anyNull) {
				string message = "All references need to be assigned!";
				EditorGUILayout.HelpBox(message, MessageType.Error);
				Debug.LogError(message);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
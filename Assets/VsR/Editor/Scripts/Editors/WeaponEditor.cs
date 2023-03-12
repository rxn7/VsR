using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	[CustomEditor(typeof(Weapon), true)]
	public class WeaponEditor : Editor {
		private SerializedProperty m_collidersProperty;
		private SerializedProperty m_dataProperty;
		private SerializedProperty m_magSlotProperty;
		private SerializedProperty m_triggerProperty;
		private SerializedProperty m_cartridgeInChamberProperty;
		private SerializedProperty m_barrelEndProperty;
		private SerializedProperty m_cartridgeEjectPointProperty;

		protected virtual void OnEnable() {
			m_collidersProperty = serializedObject.FindProperty("m_Colliders");
			m_dataProperty = serializedObject.FindProperty("m_data");
			m_magSlotProperty = serializedObject.FindProperty("m_magSlot");
			m_triggerProperty = serializedObject.FindProperty("m_trigger");
			m_cartridgeInChamberProperty = serializedObject.FindProperty("m_cartridgeInChamberObject");
			m_barrelEndProperty = serializedObject.FindProperty("m_barrelEndPoint");
			m_cartridgeEjectPointProperty = serializedObject.FindProperty("m_cartridgeEjectPoint");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			DrawProperties();

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void DrawProperties() {
			EditorGUILayout.PropertyField(m_dataProperty);
			EditorGUILayout.PropertyField(m_magSlotProperty);
			EditorGUILayout.PropertyField(m_triggerProperty);
			EditorGUILayout.PropertyField(m_cartridgeInChamberProperty);
			EditorGUILayout.PropertyField(m_barrelEndProperty);
			EditorGUILayout.PropertyField(m_cartridgeEjectPointProperty);
			EditorGUILayout.PropertyField(m_collidersProperty);
		}
	}
}

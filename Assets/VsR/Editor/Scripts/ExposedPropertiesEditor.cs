using UnityEditor;

namespace VsR.Editors {
	public class ExposedPropertiesEditor : Editor {
		protected SerializedProperty[] m_exposedProperties;

		public override void OnInspectorGUI() {
			serializedObject.UpdateIfRequiredOrScript();

			foreach (SerializedProperty prop in m_exposedProperties) {
				EditorGUILayout.PropertyField(prop);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
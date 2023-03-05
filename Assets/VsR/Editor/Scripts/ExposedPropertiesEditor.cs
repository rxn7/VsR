using UnityEditor;
using System.Collections.Generic;

namespace VsR.Editors {
	public class ExposedPropertiesEditor : Editor {
		protected List<SerializedProperty> m_exposedProperties;

		public override void OnInspectorGUI() {
			serializedObject.UpdateIfRequiredOrScript();

			foreach (SerializedProperty prop in m_exposedProperties) {
				EditorGUILayout.PropertyField(prop);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
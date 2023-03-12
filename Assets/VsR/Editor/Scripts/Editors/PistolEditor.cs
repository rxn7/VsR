using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	[CustomEditor(typeof(Pistol), true)]
	public class PistolEditor : WeaponEditor {
		private SerializedProperty m_slideProperty;

		protected override void OnEnable() {
			base.OnEnable();
			m_slideProperty = serializedObject.FindProperty("m_slide");
		}

		protected override void DrawProperties() {
			EditorGUILayout.PropertyField(m_slideProperty);
			base.DrawProperties();
		}
	}
}

using UnityEditor;
using System.Collections.Generic;

namespace VsR.Editors {
	[CustomEditor(typeof(WeaponBase), true)]
	public class WeaponBaseEditor : ExposedPropertiesEditor {
		protected virtual void OnEnable() {
			m_exposedProperties = new List<SerializedProperty> {
				serializedObject.FindProperty("m_data"),
				serializedObject.FindProperty("m_magSlot"),
				serializedObject.FindProperty("m_slide"),
				serializedObject.FindProperty("m_visualTriggerTransform"),
				serializedObject.FindProperty("m_barrelEndPoint"),
				serializedObject.FindProperty("m_cartridgeEjectPoint"),
				serializedObject.FindProperty("m_cartridgeInChamber"),
				serializedObject.FindProperty("m_AttachTransform"),
			};
		}
	}
}
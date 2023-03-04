using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	[CustomEditor(typeof(Weapon), true)]
	public class WeaponEditor : ExposedPropertiesEditor {
		private void OnEnable() {
			m_exposedProperties = new SerializedProperty[] {
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
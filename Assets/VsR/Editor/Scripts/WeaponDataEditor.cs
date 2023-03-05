using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	[CustomEditor(typeof(WeaponData), true)]
	public class WeaponDataEditor : Editor {
		private SerializedProperty m_compatibleMagazinesProp;
		private SerializedProperty m_cartridgeDataProp;

		private void OnEnable() {
			if (serializedObject != null) {
				m_compatibleMagazinesProp = serializedObject.FindProperty("compatibleMagazines");
				m_cartridgeDataProp = serializedObject.FindProperty("cartridgeData");
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			DrawDefaultInspector();

			ValidateCompatibleMagazines();

			serializedObject.ApplyModifiedProperties();
		}

		private void ValidateCompatibleMagazines() {
			CartridgeData weaponCartridgeData = (CartridgeData)m_cartridgeDataProp.objectReferenceValue;
			for (ushort i = 0; i < m_compatibleMagazinesProp.arraySize; ++i) {
				if (m_compatibleMagazinesProp.GetArrayElementAtIndex(i).objectReferenceValue is MagazineData magData) {
					if (magData.cartridgeData != weaponCartridgeData) {
						string message = $"Magazine '{magData.name}' has different CartridgeData than this weapon";
						EditorGUILayout.HelpBox(message, MessageType.Error);
						Debug.LogError(message);
					}
				}
			}
		}
	}
}
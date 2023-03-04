using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	[CustomEditor(typeof(WeaponData), true)]
	public class WeaponDataEditor : Editor {
		private SerializedProperty m_compatibleMagazinesProp;
		private SerializedProperty m_cartridgeDataProp;

		private void OnEnable() {
			m_compatibleMagazinesProp = serializedObject.FindProperty("compatibleMagazines");
			m_cartridgeDataProp = serializedObject.FindProperty("cartridgeData");
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			serializedObject.Update();

			CartridgeData weaponCartridgeData = (CartridgeData)m_cartridgeDataProp.objectReferenceValue;

			for (ushort i = 0; i < m_compatibleMagazinesProp.arraySize; ++i) {
				if (m_compatibleMagazinesProp.GetArrayElementAtIndex(i).objectReferenceValue is MagazineData magData) {
					if (magData.cartridgeData != weaponCartridgeData) {
						string message = $"Magazine '{magData.name}' has different CartridgeData than this weapon";
						GUI.contentColor = Color.red;
						EditorGUILayout.LabelField(message);
						Debug.LogError(message);
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
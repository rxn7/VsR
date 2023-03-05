using UnityEditor;

namespace VsR.Editors {
	[CustomEditor(typeof(DoubleHandedWeapon), true)]
	public class DoubleHandedWeaponEditor : WeaponBaseEditor {
		protected override void OnEnable() {
			base.OnEnable();
			m_exposedProperties.Add(serializedObject.FindProperty("m_guardHold"));
		}
	}
}
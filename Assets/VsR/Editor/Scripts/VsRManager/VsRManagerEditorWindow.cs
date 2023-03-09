using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Linq;

namespace VsR.Editors {
	public class VsRManagerEditorWindow : EditorWindow {
		private readonly VsRManagerTab[] m_tabs = new VsRManagerTab[]{
			new VsRManagerWeaponsTab(),
			new VsRManagerMagazinesTab(),
			new VsRManagerCartridgesTab(),
			// new VsRManagerSettingsTab(),
		};

		private int m_selectedTabIdx;

		private void OnEnable() {
			titleContent = new GUIContent("VsR Manager");

			foreach (VsRManagerTab tab in m_tabs)
				tab.OnEnable();
		}

		private void OnGUI() {
			string[] tabNames = m_tabs.Select(t => t.Name).ToArray();
			m_selectedTabIdx = GUILayout.Toolbar(m_selectedTabIdx, tabNames);

			EditorGUILayout.Separator();

			m_tabs[m_selectedTabIdx].Draw();
		}

		[MenuItem("VsR/Manager")]
		public static VsRManagerEditorWindow OpenWindow() {
			VsRManagerEditorWindow wnd = (VsRManagerEditorWindow)EditorWindow.GetWindow(typeof(VsRManagerEditorWindow));
			wnd.Show();

			return wnd;
		}

		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceId, int line) {
			if (!EditorWindow.HasOpenInstances<VsRManagerEditorWindow>())
				EditorWindow.CreateWindow<VsRManagerEditorWindow>();
			else
				EditorWindow.FocusWindowIfItsOpen<VsRManagerEditorWindow>();

			VsRManagerEditorWindow window = EditorWindow.GetWindow<VsRManagerEditorWindow>();

			Object obj = EditorUtility.InstanceIDToObject(instanceId);
			if (!obj) {
				Debug.LogError($"Cannot open object in VsR Manager: obj is null");
				return false;
			}

			int i = 0;
			foreach (VsRManagerTab tab in window.m_tabs) {
				if (tab is VsRManagerScriptableObjectEditorTabBase scriptableObjectEditorTab)
					if (scriptableObjectEditorTab.DataType == obj.GetType()) {
						window.m_selectedTabIdx = i;
						scriptableObjectEditorTab.Open(obj);
						return true;
					}
				++i;
			}

			return false;
		}

	}
}
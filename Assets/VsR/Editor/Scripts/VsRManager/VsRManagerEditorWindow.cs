using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Linq;

namespace VsR.Editors {
	public class VsRManagerEditorWindow : EditorWindow {
		private static readonly VsRManagerTab[] s_tabs = new VsRManagerTab[]{
			new VsRManagerWeaponsTab(),
			new VsRManagerMagazinesTab(),
			new VsRManagerCartridgesTab(),
		};

		private int m_selectedTabIdx;

		private void OnEnable() {
			titleContent = new GUIContent("VsR Manager");

			foreach (VsRManagerTab tab in s_tabs)
				tab.OnEnable();
		}

		private void OnGUI() {
			string[] tabNames = s_tabs.Select(t => t.Name).ToArray();
			m_selectedTabIdx = GUILayout.Toolbar(m_selectedTabIdx, tabNames);

			EditorGUILayout.Separator();

			s_tabs[m_selectedTabIdx].Draw();
		}

		[MenuItem("VsR/Manager")]
		public static VsRManagerEditorWindow OpenWindow() {
			VsRManagerEditorWindow wnd = (VsRManagerEditorWindow)EditorWindow.GetWindow(typeof(VsRManagerEditorWindow));
			wnd.Show();

			return wnd;
		}

		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int instanceId, int line) {
			Object obj = EditorUtility.InstanceIDToObject(instanceId);
			if (!obj)
				return false;

			int i = 0;
			foreach (VsRManagerTab tab in s_tabs) {
				if (tab is VsRManagerScriptableObjectEditorTabBase scriptableObjectEditorTab)
					if (scriptableObjectEditorTab.DataType == obj.GetType()) {
						VsRManagerEditorWindow window = EditorWindow.GetWindow<VsRManagerEditorWindow>();
						window.m_selectedTabIdx = i;
						scriptableObjectEditorTab.Open(obj);
						EditorWindow.FocusWindowIfItsOpen<VsRManagerEditorWindow>();
						return true;
					}
				++i;
			}

			return false;
		}
	}
}

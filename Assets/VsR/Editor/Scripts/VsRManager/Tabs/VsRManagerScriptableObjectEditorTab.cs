using UnityEngine;
using UnityEditor;

namespace VsR.Editors {
	public abstract class VsRManagerScriptableObjectEditorTabBase : VsRManagerTab {
		public abstract System.Type DataType { get; }
		public abstract void Open(Object obj);
	}

	public abstract class VsRManagerScriptableObjectEditorTab<T> : VsRManagerScriptableObjectEditorTabBase where T : ScriptableObject {
		public const string ROOT_DATA_PATH = "Assets/VsR/Data";
		protected Vector2 m_scrollPos;
		private SerializedObject m_serializedObject;
		private T _m_selectedObject;


		protected T SelectedObject {
			get => _m_selectedObject;
			set {
				_m_selectedObject = value;
				m_serializedObject = value != null ? new SerializedObject(value) : null;
			}
		}


		public override System.Type DataType => typeof(T);
		protected abstract Object GetPrefab();

		public override void Draw() {
			EditorGUILayout.BeginHorizontal();

			DrawList();

			EditorGUILayout.Separator();

			if (m_serializedObject != null) {
				m_serializedObject.Update();
				EditorGUILayout.BeginVertical();

				BeforeDrawInspector();
				GUILayout.Space(20);

				Editor editor = Editor.CreateEditor(SelectedObject);
				editor.DrawDefaultInspector();

				GUILayout.Space(20);
				if (GUILayout.Button("Delete") && EditorUtility.DisplayDialog($"Delete ${SelectedObject.name}", $"Are you sure you want to delete ${SelectedObject.name}?", "Yes", "No"))
					DeleteSelectedObject();

				EditorGUILayout.EndVertical();
				m_serializedObject.ApplyModifiedProperties();
			} else {
				GUILayout.Label("Select an object from the list");
			}

			EditorGUILayout.EndHorizontal();
		}

		public override void Open(Object obj) {
			SelectedObject = (T)obj;
		}

		protected virtual void BeforeDrawInspector() {
			if (GUILayout.Button("Open prefab"))
				AssetDatabase.OpenAsset(GetPrefab());
		}

		public void DrawList() {
			EditorGUILayout.BeginVertical(GUILayout.MaxWidth(200));

			if (GUILayout.Button("Create new"))
				CreateNewDataInstance();

			EditorGUILayout.Separator();

			T[] objects = Resources.FindObjectsOfTypeAll<T>();
			m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, false, false);

			foreach (T obj in objects) {
				GUI.enabled = obj != SelectedObject;
				if (GUILayout.Button(obj.name))
					SelectedObject = obj;
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
			GUI.enabled = true;
		}

		protected virtual void CreateNewDataInstance() {
			string name = EditorInputDialog.Show($"Create new {typeof(T).Name}", $"Enter {typeof(T).Name}'s name", typeof(T).Name);
			if (string.IsNullOrEmpty(name))
				return;

			T instance = (T)ScriptableObject.CreateInstance(DataType);
			instance.name = name;

			if (!AssetDatabase.IsValidFolder("Assets/VsR"))
				AssetDatabase.CreateFolder("Assets", "VsR");

			if (!AssetDatabase.IsValidFolder("Assets/VsR/Data"))
				AssetDatabase.CreateFolder("Assets/VsR", "Data");

			if (!AssetDatabase.IsValidFolder($"Assets/VsR/Data/{Name}"))
				AssetDatabase.CreateFolder("Assets/VsR/Data", Name);

			string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/VsR/Data/{Name}/{name}.asset");
			AssetDatabase.CreateAsset(instance, path);
			AssetDatabase.SaveAssets();
		}

		protected virtual void DeleteSelectedObject() {
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedObject));
			SelectedObject = null;
		}
	}
}
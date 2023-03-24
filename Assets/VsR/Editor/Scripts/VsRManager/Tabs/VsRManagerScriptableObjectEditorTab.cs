using UnityEngine;
using UnityEditor;
using System.IO;

namespace VsR.Editors {
	public abstract class VsRManagerScriptableObjectEditorTabBase : VsRManagerTab {
		public abstract System.Type DataType { get; }
		public abstract void Open(Object obj);
	}

	public abstract class VsRManagerScriptableObjectEditorTab<T> : VsRManagerScriptableObjectEditorTabBase where T : ScriptableObject {
		public const string ROOT_DATA_PATH = "Assets/VsR/Data";
		protected Vector2 m_listScrollPos;
		private SerializedObject m_serializedObject = null;
		private T _m_selectedObject = null;

		protected T SelectedObject {
			get => _m_selectedObject;
			set {
				_m_selectedObject = value;
				if (value != null) {
					m_serializedObject = new SerializedObject(value);
					OnObjectSelected();
				} else {
					m_serializedObject = null;
				}
			}
		}

		public override System.Type DataType => typeof(T);
		protected abstract Object Prefab { get; set; }
		protected abstract bool DeleteWithPrefab { get; }
		protected abstract string PrefabPath { get; }
		protected virtual void OnObjectSelected() { }
		protected Vector2 m_inspectorScrollPos;

		public override void Draw() {
			EditorGUILayout.BeginHorizontal();

			DrawList();

			EditorGUILayout.Separator();

			if (SelectedObject) {
				m_serializedObject.Update();
				EditorGUILayout.BeginVertical();

				BeforeDrawInspector();
				GUILayout.Space(20);

				m_inspectorScrollPos = EditorGUILayout.BeginScrollView(m_inspectorScrollPos);

				Editor editor = Editor.CreateEditor(SelectedObject);
				editor.DrawDefaultInspector();

				m_serializedObject.ApplyModifiedProperties();

				EditorGUILayout.EndScrollView();

				bool deleted = false;
				GUILayout.Space(20);
				if (deleted = GUILayout.Button("Delete") && EditorUtility.DisplayDialog($"Delete ${SelectedObject.name}", $"Are you sure you want to delete ${SelectedObject.name}?", "Yes", "No"))
					DeleteSelectedObject();

				EditorGUILayout.EndVertical();
			} else {
				EditorGUILayout.HelpBox("Select an object from the list", MessageType.Info);
			}

			EditorGUILayout.EndHorizontal();
		}

		public override void Open(Object obj) {
			SelectedObject = (T)obj;
		}

		protected virtual void BeforeDrawInspector() {
			if (Prefab) {
				if (GUILayout.Button("Open prefab"))
					AssetDatabase.OpenAsset(Prefab);
			} else {
				if (GUILayout.Button("Create prefab"))
					CreatePrefab();
			}
		}

		public void DrawList() {
			EditorGUILayout.BeginVertical(GUILayout.MaxWidth(200));

			if (GUILayout.Button("Create new"))
				CreateNewDataInstance();

			EditorGUILayout.Separator();

			T[] objects = Resources.FindObjectsOfTypeAll<T>();
			m_listScrollPos = EditorGUILayout.BeginScrollView(m_listScrollPos, false, false);

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

			string path = $"Assets/VsR/Data/{Name}/{name}.asset";
			EditorHelper.CreateAssetSafe(instance, path);
		}

		protected virtual void DeleteSelectedObject() {
			AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(SelectedObject));

			if (DeleteWithPrefab && Prefab)
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(Prefab));

			SelectedObject = null;
		}

		protected void CreatePrefab() {
			if (!Directory.Exists(PrefabPath))
				Directory.CreateDirectory(PrefabPath);

			GameObject obj = new GameObject(SelectedObject.name);

			Prefab = PrefabUtility.SaveAsPrefabAsset(obj, $"{PrefabPath}/{SelectedObject.name}.prefab");
			AssetDatabase.OpenAsset(Prefab);
		}
	}
}
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

namespace VsR {
	public class MagazineSpawner : XRSimpleInteractable {
		[SerializeField]
		private MagazineData _m_data;

		public MagazineData Data {
			get => _m_data;
			set {
				_m_data = value;
				UpdateText();
			}
		}

		protected override void Awake() {
			base.Awake();
			UpdateText();
		}

		private void UpdateText() {
			GetComponentInChildren<TMPro.TextMeshPro>().text = _m_data.name;
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);

			interactionManager.SelectExit(args.interactorObject, this);

			Magazine mag = Instantiate(_m_data.prefab);
			mag.transform.position = args.interactorObject.GetAttachTransform(mag).position;

			interactionManager.SelectEnter(args.interactorObject, mag);
		}
	}
}

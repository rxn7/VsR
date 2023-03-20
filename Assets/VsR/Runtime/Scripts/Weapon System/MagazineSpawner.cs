using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class MagazineSpawner : XRSimpleInteractable {
		[SerializeField] private MagazineData m_data;

		public MagazineData Data {
			get => m_data;
			set {
				m_data = value;
				UpdateText();
			}
		}

		protected override void Awake() {
			base.Awake();
			UpdateText();
		}

		private void UpdateText() {
			GetComponentInChildren<TMPro.TextMeshPro>().text = m_data.name;
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);

			interactionManager.SelectExit(args.interactorObject, this);

			Magazine mag = Instantiate(m_data.prefab);
			mag.transform.position = args.interactorObject.GetAttachTransform(mag).position;

			interactionManager.SelectEnter(args.interactorObject, mag);
		}
	}
}

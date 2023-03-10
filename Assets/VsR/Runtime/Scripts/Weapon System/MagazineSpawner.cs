using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class MagazineSpawner : XRSimpleInteractable {
		[SerializeField] private MagazineData m_magazineData;
		[SerializeField] private TMPro.TMP_Text m_text;

		protected override void Awake() {
			base.Awake();
			m_text.text = m_magazineData.name + " Spawner";
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);

			interactionManager.SelectExit(args.interactorObject, this);

			Magazine mag = Instantiate(m_magazineData.prefab);
			mag.transform.position = args.interactorObject.GetAttachTransform(mag).position;

			interactionManager.SelectEnter(args.interactorObject, mag);
		}
	}
}

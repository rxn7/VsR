using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class MagazineSlot : XRSocketInteractor {
		[SerializeField] private WeaponBase m_weapon;
		private Magazine m_magazine;

		public Magazine Mag => m_magazine;

		protected override void Awake() {
			base.Awake();

			showInteractableHoverMeshes = false;

			recycleDelayTime = 0.3f;
			TimeManager.onTimeScaleChanged += (float scale) => recycleDelayTime = 0.3f * scale;
		}

		public override bool CanSelect(IXRSelectInteractable interactable) {
			bool isSelecting = IsSelecting(interactable);

			if (!m_weapon.isSelected && !isSelecting)
				return false;

			if (hasSelection && !isSelecting)
				return false;

			if (!isSelecting && interactable.firstInteractorSelecting is not Hand)
				return false;

			if (interactable is not Magazine mag)
				return false;

			if (!m_weapon.Data.compatibleMagazines.Contains(mag.Data))
				return false;

			return base.CanHover((IXRHoverInteractable)interactable);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			if (args.interactableObject is not Magazine mag)
				throw new System.ArgumentException("Interactable is not a Magazine!");

			OnMagazineSelected(mag);
		}

		private void OnMagazineSelected(Magazine mag) {
			m_magazine = mag;

			SoundPoolManager.Instance.PlaySound(m_magazine.Data.slideInSound, transform.position, Random.Range(0.95f, 1.05f));

			mag.SlideIn();
		}

		public void ReleaseMagazine() {
			if (!m_magazine || !IsSelecting(m_magazine))
				return;

			SoundPoolManager.Instance.PlaySound(m_magazine.Data.slideOutSound, transform.position, Random.Range(0.95f, 1.05f));

			interactionManager.SelectExit(this, (IXRSelectInteractable)m_magazine);
			m_magazine.SlideOut();

			m_magazine = null;
		}
	}
}
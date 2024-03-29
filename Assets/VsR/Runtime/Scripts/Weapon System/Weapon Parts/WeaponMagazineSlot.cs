using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VsR {
	public class WeaponMagazineSlot : XRSocketInteractor, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
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

			if (!Weapon.GripHand && !isSelecting)
				return false;

			if (hasSelection && !isSelecting)
				return false;

			if (!isSelecting && interactable.firstInteractorSelecting is not Hand)
				return false;

			if (interactable is not Magazine mag)
				return false;

			if (!Weapon.Data.compatibleMagazines.Contains(mag.Data))
				return false;

			return base.CanHover((UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable)interactable);
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

			interactionManager.SelectExit(this, (UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)m_magazine);
			m_magazine.SlideOut(Weapon.VelocityTracker.velocity);

			m_magazine = null;
		}
	}
}
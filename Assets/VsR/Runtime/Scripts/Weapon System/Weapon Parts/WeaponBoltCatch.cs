using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class WeaponBoltCatch : WeaponPart {
		[SerializeField] private AudioClip m_boltReleaseClip;
		[SerializeField] public WeaponBolt m_bolt;
		[SerializeField] public WeaponSlide m_slide;
		[SerializeField] private MagazineSlot m_magSlot;

		protected override void Awake() {
			base.Awake();
			m_slide.onRelease += OnSlideRelease;
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) => false;
		public override bool IsHoverableBy(IXRHoverInteractor interactor) {
			if (interactor is not Hand)
				return false;

			return base.IsHoverableBy(interactor);
		}

		protected override void OnHoverEntered(HoverEnterEventArgs args) {
			base.OnHoverEntered(args);
			interactionManager.HoverExit(args.interactorObject, this);

			if (!m_bolt.IsOpen) return;
			CloseBolt();
		}

		public void CloseBolt() {
			m_bolt.IsOpen = false;
			m_weapon.TryToCock();

			SoundPoolManager.Instance.PlaySound(m_boltReleaseClip, transform.position, Random.Range(0.9f, 1.1f));
		}

		private void OnSlideRelease() {
			if (m_slide.Racked)
				m_bolt.IsOpen = false;
		}
	}
}
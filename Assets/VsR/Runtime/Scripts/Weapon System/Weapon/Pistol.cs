using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	public class Pistol : SingleHandedWeapon {
		private static AudioClip s_slideStopClip;

		public bool SlideStop {
			get => m_animator.GetBool("SlideStop");
			set => m_animator.SetBool("SlideStop", value);
		}

		protected override void Awake() {
			base.Awake();
			if (!s_slideStopClip)
				s_slideStopClip = Resources.Load<AudioClip>("Audio/slide_stop");
		}

		protected override void Fire() {
			base.Fire();

			if (!CartridgeInChamber)
				m_animator.SetBool("SlideStop", true);
		}

		protected override void OnCocked() {
			base.OnCocked();
			m_animator.SetBool("SlideStop", false);
		}


		protected override bool CanFire() {
			if (SlideStop)
				return false;

			return base.CanFire();
		}

		protected override void OnSlideReleasePressed(InputAction.CallbackContext context) {
			base.OnSlideReleasePressed(context);
			if (m_slide.isSelected && m_slide.Racked) {
				SoundPoolManager.Instance.PlaySound(s_slideStopClip, m_slide.transform.position, Random.Range(0.9f, 1.1f));
				SlideStop = true;
				interactionManager.SelectExit(m_slide.firstInteractorSelecting, m_slide);
			} else if (SlideStop) {
				SoundPoolManager.Instance.PlaySound(Data.rackBackSound, m_slide.transform.position, Random.Range(0.9f, 1.1f));
				SlideStop = false;
				if (!CartridgeInChamber)
					TryToCock();
			}
		}
	}
}
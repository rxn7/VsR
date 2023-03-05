using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	public class Pistol : SingleHandedWeapon {
		public bool SlideStop {
			get => m_animator.GetBool("SlideStop");
			set => m_animator.SetBool("SlideStop", value);
		}

		protected override void Fire() {
			base.Fire();

			if (!BulletInChamber)
				m_animator.SetBool("SlideStop", true);
		}

		protected override void OnCocked() {
			base.OnCocked();
			m_animator.SetBool("SlideStop", false);
		}

		protected override void OnSlideStopPressed(InputAction.CallbackContext context) {
			base.OnSlideStopPressed(context);
			if (SlideStop) {
				SoundManager.Instance.PlaySound(Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
				SlideStop = false;
				TryToCock();
			}
		}
	}
}
using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	public class Pistol : Weapon {
		public bool SlideStop {
			get => Animator.GetBool("SlideBack");
			set => Animator.SetBool("SlideBack", value);
		}

		protected override void Fire() {
			base.Fire();

			if (!BulletInChamber)
				Animator.SetBool("SlideStop", true);
		}

		protected override void OnCocked() {
			base.OnCocked();
			Animator.SetBool("SlideStop", false);
		}

		protected override void OnSlideStopPressed(InputAction.CallbackContext context) {
			base.OnSlideStopPressed(context);
			if (SlideStop) {
				SoundManager.Instance.PlaySound(Data.cockBackSound, transform.position, Random.Range(0.9f, 1.1f));
				SlideStop = false;
				TryToCock();
			}
		}
	}
}
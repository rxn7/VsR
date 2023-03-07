using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	public class Pistol : WeaponBase {
		[SerializeField] private PistolSlide m_slide;
		private static AudioClip s_slideStopSound;

		[RuntimeInitializeOnLoadMethod]
		private static void Init() {
			s_slideStopSound = Resources.Load<AudioClip>("Audio/slide_stop");
		}

		protected override void Awake() {
			base.Awake();
		}

		protected override void OnSlideReleasePressed(InputAction.CallbackContext context) {
			base.OnSlideReleasePressed(context);
			if (m_slide.isSelected && m_slide.Racked) {
				(m_slide).Locked = true;
				interactionManager.SelectExit(m_slide.firstInteractorSelecting, m_slide);

				SoundPoolManager.Instance.PlaySound(s_slideStopSound, m_slide.transform.position, Random.Range(0.9f, 1.1f));
			} else if ((m_slide).Locked) {
				(m_slide).Locked = false;
				m_slide.OnRelease();

				if (!CartridgeInChamber)
					TryToCock();

				SoundPoolManager.Instance.PlaySound(Data.rackBackSound, m_slide.transform.position, Random.Range(0.9f, 1.1f));
			}
		}
	}
}
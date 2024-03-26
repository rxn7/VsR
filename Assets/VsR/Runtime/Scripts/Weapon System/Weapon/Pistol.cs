using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	public class Pistol : Weapon {
		[SerializeField] private WeaponSlide m_slide;
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
			if (m_slide.isSelected && m_slide.IsPulledBack) {
				m_slide.Bolt.IsOpen = true;
				interactionManager.SelectExit(m_slide.firstInteractorSelecting, m_slide);
				SoundPoolManager.Instance.PlaySound(s_slideStopSound, m_slide.transform.position, Random.Range(0.9f, 1.1f));
			} else if (m_slide.Bolt.IsOpen) {
				m_slide.Bolt.IsOpen = false;
				m_slide.Release();

				if (!chamberedCartridge)
					Rack();

				SoundPoolManager.Instance.PlaySound(Data.rackSound, m_slide.transform.position, Random.Range(0.9f, 1.1f));
			}
		}
	}
}
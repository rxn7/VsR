using UnityEngine;

namespace VsR {
	public class WeaponBoltCatch : MonoBehaviour, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		[SerializeField] private AudioClip m_boltReleaseClip;
		[SerializeField] public WeaponBolt m_bolt;
		[SerializeField] public WeaponSlide m_slide;
		[SerializeField] private WeaponMagazineSlot m_magSlot;

		protected void Awake() {
			m_slide.onRelease += OnSlideRelease;
		}

		protected void OnTriggerEnter(Collider collider) {
			if (!collider.gameObject.TryGetComponent<Hand>(out Hand hand) || !m_bolt.IsOpen || !Weapon.isSelected)
				return;

			if (hand.interactablesSelected.Contains(Weapon))
				return;

			CloseBolt();
		}

		public void CloseBolt() {
			m_bolt.IsOpen = false;
			Weapon.TryToCock();

			SoundPoolManager.Instance.PlaySound(m_boltReleaseClip, transform.position, Random.Range(0.9f, 1.1f));
		}

		private void OnSlideRelease() {
			if (m_slide.Racked)
				m_bolt.IsOpen = false;
		}
	}
}
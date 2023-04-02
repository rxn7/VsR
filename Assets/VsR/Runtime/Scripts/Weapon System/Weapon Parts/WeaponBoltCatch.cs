using UnityEngine;

namespace VsR {
	public class WeaponBoltCatch : MonoBehaviour, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		[SerializeField] private AudioClip m_sound;
		[SerializeField] private HapticFeedback m_feedback = new HapticFeedback(0.3f, 0.1f);
		[SerializeField] public WeaponBolt m_bolt;
		[SerializeField] private WeaponMagazineSlot m_magSlot;

		private void Awake() {
			IWeaponPart.Validate(this);
		}

		protected void OnTriggerEnter(Collider collider) {
			if (!collider.gameObject.TryGetComponent<Hand>(out Hand hand) || !m_bolt.IsOpen || !Weapon.GripHand)
				return;

			if (hand.interactablesSelected.Contains(Weapon))
				return;

			CloseBolt();
		}

		public void CloseBolt() {
			Weapon.GripHand?.ApplyHapticFeedback(m_feedback);

			m_bolt.IsOpen = false;
			Weapon.TryToCock();

			SoundPoolManager.Instance.PlaySound(m_sound, transform.position, Random.Range(0.9f, 1.1f));
		}
	}
}
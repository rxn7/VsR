using UnityEngine;

namespace VsR {
	public class WeaponBoltCatch : MonoBehaviour, IWeaponPart {
		[field: SerializeField] public Weapon Weapon { get; set; }
		[SerializeField] private AudioClip m_boltReleaseClip;
		[SerializeField] public WeaponBolt m_bolt;
		[SerializeField] private WeaponMagazineSlot m_magSlot;

		protected void OnTriggerEnter(Collider collider) {
			if (!collider.gameObject.TryGetComponent<Hand>(out Hand hand) || !m_bolt.IsOpen || !Weapon.GripHand)
				return;

			if (hand.interactablesSelected.Contains(Weapon))
				return;

			CloseBolt();
		}

		public void CloseBolt() {
			m_bolt.IsOpen = false;
			Weapon.Rack();

			SoundPoolManager.Instance.PlaySound(m_boltReleaseClip, transform.position, Random.Range(0.9f, 1.1f));
		}
	}
}
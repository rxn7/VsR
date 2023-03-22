using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	[RequireComponent(typeof(CollisionSound))]
	public class Magazine : XRGrabInteractable {
		public const float EMPTY_MAG_DROP_DESTROY_DELAY_SECS = 10.0f;

		[Header("Magazine")]
		[SerializeField] private MagazineData m_data;
		[SerializeField] private GameObject m_bulletObject;

		private uint _m_bulletCount;
		private Rigidbody m_rb;

		public uint bulletCount {
			get => _m_bulletCount;
			set {
				_m_bulletCount = value;
				m_bulletObject?.SetActive(value > 0);
			}
		}
		public bool IsEmpty => _m_bulletCount == 0;
		public MagazineData Data => m_data;

		protected override void Awake() {
			base.Awake();

			if (!m_data) {
				Destroy(gameObject);
				throw new UnassignedReferenceException($"m_data is not assigned on {nameof(Magazine)}!");
			}

			m_rb = GetComponent<Rigidbody>();
			m_rb.interpolation = RigidbodyInterpolation.Interpolate;
			m_rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

			bulletCount = m_data.capacity;
		}

		public void SlideIn() {
			m_rb.isKinematic = true;
			m_rb.detectCollisions = false;
		}

		public void SlideOut(Vector3 weaponVelocity) {
			m_rb.isKinematic = false;
			m_rb.detectCollisions = true;
			m_rb.velocity = -transform.up * 0.3f + weaponVelocity;
		}

		protected override void Drop() {
			base.Drop();
			if (IsEmpty)
				Destroy(gameObject, EMPTY_MAG_DROP_DESTROY_DELAY_SECS);
		}
	}
}
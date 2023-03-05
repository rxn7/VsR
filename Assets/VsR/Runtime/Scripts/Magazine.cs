using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

namespace VsR {
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
				if (m_bulletObject != null)
					m_bulletObject.SetActive(value > 0);
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

		public void SlideOut() {
			m_rb.isKinematic = false;
			m_rb.detectCollisions = true;
			m_rb.AddForce(-transform.up * 0.3f, ForceMode.Impulse);
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (firstInteractorSelecting is MagazineSlot && firstInteractorSelecting != interactor)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void Drop() {
			base.Drop();
			if (IsEmpty)
				Destroy(gameObject, EMPTY_MAG_DROP_DESTROY_DELAY_SECS);
		}
	}
}
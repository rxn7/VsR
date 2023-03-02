using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Magazine : XRGrabInteractable {
	[Space]
	[Header("Magazine")]
	[SerializeField] private MagazineData m_data;

	[HideInInspector] public uint bulletCount;
	private Rigidbody m_rb;

	public MagazineData Data => m_data;

	protected override void Awake() {
		base.Awake();

		if (m_data == null) {
			Destroy(gameObject);
			throw new UnassignedReferenceException($"m_data is not assigned on {nameof(Magazine)}!");
		}

		m_rb = GetComponent<Rigidbody>();
		m_rb.interpolation = RigidbodyInterpolation.Interpolate;

		bulletCount = m_data.capacity;
	}

	public override bool IsSelectableBy(IXRSelectInteractor interactor) {
		if (firstInteractorSelecting is MagazineSlot && firstInteractorSelecting != interactor)
			return false;

		return base.IsSelectableBy(interactor);
	}

	public void EnterWeapon() {
		m_rb.isKinematic = true;
		m_rb.detectCollisions = false;
	}

	public void Release() {
		m_rb.isKinematic = false;
		m_rb.detectCollisions = true;
		m_rb.AddForce(-transform.up * 1, ForceMode.Impulse);
	}
}

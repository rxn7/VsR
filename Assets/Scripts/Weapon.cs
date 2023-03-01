using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : XRGrabInteractable {
	[Header("Weapon")]
	[SerializeField] private WeaponData m_data;
	[SerializeField] private Transform m_triggerTransform;
	[SerializeField] private Transform m_bulletSpawnPoint;

	private Animator m_animator;
	private Hand m_hand = null;
	private bool m_triggerReset = true;
	private ushort m_ammoInMag;

	private bool CanFire => m_triggerReset && m_ammoInMag > 0;

	protected override void Awake() {
		base.Awake();

		if (m_data == null) {
			Destroy(gameObject);
			throw new UnassignedReferenceException("m_data is not assigned on Weapon!");
		}

		m_ammoInMag = m_data.ammoPerMag;
		m_animator = GetComponent<Animator>();

		SetTriggerValue(0);
	}

	private void Update() {
		if (!isSelected)
			return;

		UpdateTrigger();
	}

	public override bool IsSelectableBy(IXRSelectInteractor interactor) {
		if (interactor is not Hand)
			return false;

		if (m_hand != null && (IXRSelectInteractor)m_hand != interactor)
			return false;

		return base.IsSelectableBy(interactor);
	}

	protected override void OnSelectEntering(SelectEnterEventArgs args) {
		base.OnSelectEntering(args);
		m_hand = (Hand)args.interactorObject;
	}

	protected override void OnSelectExited(SelectExitEventArgs args) {
		base.OnSelectExited(args);
		SetTriggerValue(0);
		m_hand = null;
	}

	private void Fire() {
		m_triggerReset = false;

		// m_ammoInMag--; TODO: Uncomment this when reloading system is done

		m_animator.SetTrigger("Shoot");
		m_hand.xrController.SendHapticImpulse(m_data.shootHapticFeedbackIntensity, m_data.shootHapticFeedbackDuration);
		SoundManager.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));
		SpawnBullet();
	}

	private void SpawnBullet() {
		Bullet bullet = GameObject.Instantiate<Bullet>(m_data.bulletPrefab);
		bullet.transform.position = m_bulletSpawnPoint.position;
		bullet.ApplyForce(m_bulletSpawnPoint.forward, m_data.force);
	}

	private void UpdateTrigger() {
		if (m_hand == null)
			return;

		SetTriggerValue(m_hand.TriggerAction.ReadValue<float>());
	}

	private void SetTriggerValue(float normalizedTriggerValue) {
		float triggerRotation = Mathf.Lerp(m_data.minTriggerRotation, m_data.maxTriggerRotation, normalizedTriggerValue);

		if (m_triggerTransform != null)
			m_triggerTransform.localEulerAngles = new Vector3(triggerRotation, 0, 0);

		if (CanFire && normalizedTriggerValue >= m_data.fireTriggerValue)
			Fire();

		if (m_data.type == WeaponData.WeaponType.SemiAutomatic && normalizedTriggerValue <= m_data.fireTriggerValue * 0.9)
			m_triggerReset = true;
	}
}

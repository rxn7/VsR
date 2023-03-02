using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Weapon : XRGrabInteractable {
	[Space]
	[Header("Weapon")]
	[SerializeField] private WeaponData m_data;
	[SerializeField] private MagazineSlot m_magSlot;
	[SerializeField] private Transform m_triggerTransform;
	[SerializeField] private Transform m_bulletSpawnPoint;
	[SerializeField] private Transform m_bulletCaseEjectPoint;

	private Animator m_animator;
	private Hand m_hand = null;
	private bool m_triggerReset = true;
	private bool m_cocked = false;

	private bool CanFire => m_triggerReset && m_cocked;
	public WeaponData Data => m_data;

	protected override void Awake() {
		base.Awake();

		if (m_data == null) {
			Destroy(gameObject);
			throw new UnassignedReferenceException($"m_data is not assigned on {nameof(Weapon)}!");
		}

		if (m_magSlot == null) {
			Destroy(gameObject);
			throw new UnassignedReferenceException($"m_magSlot is not assigned on {nameof(Weapon)}!");
		}

		m_magSlot.weapon = this;

		m_animator = GetComponent<Animator>();

		SetTriggerValue(0);
	}

	private void Update() {
		if (!isSelected)
			return;

		UpdateTrigger();
	}

	private void Fire() {
		m_cocked = false;

		SpawnBullet();
		EjectBulletCase();

		m_animator.SetTrigger("Shoot");
		m_hand.xrController.SendHapticImpulse(m_data.shootHapticFeedbackIntensity, m_data.shootHapticFeedbackDuration);
		SoundManager.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));

		switch (m_data.fireMode) {
			case WeaponData.FireMode.Automatic:
			case WeaponData.FireMode.SemiAutomatic:
				Cock();
				break;

			default:
				break;
		}
	}

	public void OnMagazineEntered(Magazine mag) {
		Cock(); // TODO: delete this later
	}

	private void SpawnBullet() {
		Bullet bullet = GameObject.Instantiate<Bullet>(m_data.bullet.bulletPrefab);
		bullet.transform.position = m_bulletSpawnPoint.position;
		bullet.ApplyForce(m_bulletSpawnPoint.forward, m_data.force);
	}

	private void EjectBulletCase() {
		BulletCase bulletCase = Instantiate(Data.bullet.bulletCasePrefab, m_bulletCaseEjectPoint.position, m_bulletCaseEjectPoint.rotation);
		bulletCase.Eject();
	}

	private void Cock() {
		// TODO: Eject bullet if is already cocked
		if (m_cocked)
			return;

		if (m_magSlot.Mag.bulletCount < 1)
			return;

		m_magSlot.Mag.bulletCount--;
		m_cocked = true;
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

		UpdateTriggerReset(normalizedTriggerValue);
	}

	private void UpdateTriggerReset(float normalizedTriggerValue) {
		switch (m_data.fireMode) {
			case WeaponData.FireMode.Automatic:
				m_triggerReset = true;
				break;

			case WeaponData.FireMode.SemiAutomatic:
				m_triggerReset = normalizedTriggerValue < m_data.fireTriggerValue;
				break;

			case WeaponData.FireMode.Manual:
				m_triggerReset = false;
				break;
		}
	}


	private void ReleaseMagazine(InputAction.CallbackContext context) {
		m_magSlot.ReleaseMagazine();
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
		m_hand.MagReleaseAction.performed += ReleaseMagazine;
	}

	protected override void OnSelectExited(SelectExitEventArgs args) {
		base.OnSelectExited(args);
		SetTriggerValue(0);
		m_hand.MagReleaseAction.performed -= ReleaseMagazine;
		m_hand = null;
	}
}

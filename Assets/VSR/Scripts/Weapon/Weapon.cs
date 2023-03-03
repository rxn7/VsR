using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class Weapon : XRGrabInteractable {
		[Space]
		[Header("Weapon")]
		[SerializeField] private WeaponData m_data;
		[SerializeField] private MagazineSlot m_magSlot;
		[SerializeField] private WeaponSlide m_slide;
		[SerializeField] private Transform m_triggerTransform;
		[SerializeField] private Transform m_bulletSpawnPoint;
		[SerializeField] private Transform m_bulletCaseEjectPoint;

		public Animator Animator { get; private set; }
		public Hand GripHand { get; private set; } = null;
		private bool m_triggerReleased = true;
		private bool m_cocked = false;

		public WeaponData Data => m_data;

		protected override void Awake() {
			base.Awake();

			if (m_data == null) {
				Destroy(gameObject);
				throw new UnassignedReferenceException($"m_data is not assigned!");
			}

			if (m_magSlot == null) {
				Destroy(gameObject);
				throw new UnassignedReferenceException($"m_magSlot is not assigned!");
			}

			m_magSlot.weapon = this;

			Animator = GetComponent<Animator>();

			SetTriggerValue(0);
		}

		private void Update() {
			if (!isSelected)
				return;

			UpdateTrigger();
		}

		private void Fire() {
			m_cocked = false;

			Animator.SetTrigger("Shoot");
			GripHand.xrController.SendHapticImpulse(m_data.shootHapticFeedbackIntensity, m_data.shootHapticFeedbackDuration);
			SoundManager.Instance.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));

			switch (m_data.fireMode) {
				case WeaponData.FireMode.Automatic:
				case WeaponData.FireMode.SemiAutomatic:
					Cock();
					break;

				default:
					break;
			}

			if (!m_cocked)
				Animator.SetBool("SlideStop", true);
		}

		private void DryFire() {
			SoundManager.Instance.PlaySound(m_data.dryFireSound, transform.position, Random.Range(0.9f, 1.1f));
		}

		// This should be triggered from animation
		private void FireBullet() {
			Bullet bullet = GameObject.Instantiate<Bullet>(m_data.bullet.bulletPrefab);
			bullet.transform.position = m_bulletSpawnPoint.position;
			bullet.ApplyForce(m_bulletSpawnPoint.forward, m_data.force);
		}

		// This should be triggered from animation
		private void EjectBulletCase() {
			BulletCase bulletCase = Instantiate(Data.bullet.bulletCasePrefab, m_bulletCaseEjectPoint.position, m_bulletCaseEjectPoint.rotation);
			bulletCase.Eject();
		}

		private bool CanFire() {
			switch (m_data.fireMode) {
				case WeaponData.FireMode.Automatic:
					// TODO:
					break;

				case WeaponData.FireMode.SemiAutomatic:
					if (!m_triggerReleased)
						return false;
					break;

				case WeaponData.FireMode.Manual:
					// TODO:
					break;
			}

			if (m_slide != null && m_slide.isSelected)
				return false;

			if (!m_cocked) {
				DryFire();
				return false;
			}


			return true;
		}

		public void Cock() {
			if (m_cocked) {
				EjectBulletCase(); // TODO: Eject full round instead of just case
				m_cocked = false;
			}

			if (m_magSlot.Mag == null || m_magSlot.Mag.bulletCount == 0)
				return;

			m_magSlot.Mag.bulletCount--;
			m_cocked = true;
			Animator.SetBool("SlideStop", false);
		}

		private void UpdateTrigger() {
			if (GripHand == null)
				return;

			SetTriggerValue(GripHand.TriggerAction.ReadValue<float>());
		}

		private void SetTriggerValue(float normalizedTriggerValue) {
			float triggerRotation = Mathf.Lerp(m_data.minTriggerRotation, m_data.maxTriggerRotation, normalizedTriggerValue);

			if (m_triggerTransform != null)
				m_triggerTransform.localEulerAngles = new Vector3(triggerRotation, 0, 0);

			if (normalizedTriggerValue >= m_data.fireTriggerValue && CanFire())
				Fire();

			UpdateTriggerReset(normalizedTriggerValue);
		}

		private void UpdateTriggerReset(float normalizedTriggerValue) {
			switch (m_data.fireMode) {
				case WeaponData.FireMode.Automatic:
					m_triggerReleased = true;
					break;

				case WeaponData.FireMode.SemiAutomatic:
					m_triggerReleased = normalizedTriggerValue < m_data.fireTriggerValue;
					break;

				case WeaponData.FireMode.Manual:
					m_triggerReleased = false;
					break;
			}
		}

		private void ReleaseMagazine(InputAction.CallbackContext context) {
			m_magSlot.ReleaseMagazine();
		}

		private void ReleaseSlide(InputAction.CallbackContext context) {
			if (Animator.GetBool("SlideStop")) {
				SoundManager.Instance.PlaySound(Data.cockBackSound, transform.position, Random.Range(0.9f, 1.1f));
				Animator.SetBool("SlideStop", false);
				Cock();
			}
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (GripHand != null && (IXRSelectInteractor)GripHand != interactor)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntering(SelectEnterEventArgs args) {
			base.OnSelectEntering(args);
			GripHand = (Hand)args.interactorObject;
			GripHand.MagReleaseAction.performed += ReleaseMagazine;
			GripHand.SlideReleaseAction.performed += ReleaseSlide;
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);
			SetTriggerValue(0);
			GripHand.MagReleaseAction.performed -= ReleaseMagazine;
			GripHand.SlideReleaseAction.performed -= ReleaseSlide;
			GripHand = null;
		}
	}
}
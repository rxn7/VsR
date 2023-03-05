using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class Weapon : XRGrabInteractable {
		[SerializeField] private WeaponData m_data;
		[SerializeField] private MagazineSlot m_magSlot;
		[SerializeField] private WeaponSlide m_slide;
		[SerializeField] private Transform m_visualTriggerTransform;
		[SerializeField] private GameObject m_cartridgeInChamber;
		[SerializeField] private Transform m_barrelEndPoint;
		[SerializeField] private Transform m_cartridgeEjectPoint;

		public Animator Animator { get; private set; }
		public Hand GripHand { get; private set; } = null;
		private float m_fireRateTimer = 0.0f;
		private bool m_triggerReset = true;
		private bool _m_bulletInChamber;
		public bool BulletInChamber {
			get => _m_bulletInChamber;
			private set {
				_m_bulletInChamber = value;
				m_cartridgeInChamber.SetActive(value);
			}
		}

		public WeaponData Data => m_data;

		protected override void Awake() {
			base.Awake();

			m_magSlot.weapon = this;
			m_slide.weapon = this;

			Animator = GetComponent<Animator>();

			UpdateTriggerValue(0);
			BulletInChamber = false;
		}

		protected virtual void Update() {
			if (!isSelected)
				return;

			m_fireRateTimer += Time.deltaTime;
		}

		private void OnDrawGizmos() {
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(m_barrelEndPoint.position, 0.01f);

			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(m_cartridgeEjectPoint.position, m_cartridgeEjectPoint.position + m_cartridgeEjectPoint.up * 0.1f);
		}

		protected virtual void DryFire() {
			SoundManager.Instance.PlaySound(m_data.dryFireSound, transform.position, Random.Range(0.9f, 1.1f));
			m_triggerReset = false;
		}

		protected virtual void Fire() {
			BulletInChamber = false;
			m_triggerReset = false;
			m_fireRateTimer = 0.0f;

			Animator.SetTrigger("Shoot");
			GripHand.ApplyHapticFeedback(m_data.fireHapticFeedback);
			SoundManager.Instance.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));

			FireProjectile();

			// Automatic & SemiAutomatic weapons' slide moves back from recoil
			if (Data.shootType != WeaponData.ShootType.Manual)
				TryToCock();
		}

		protected virtual bool CanFire() {
			if (Data.shootType != WeaponData.ShootType.Automatic && !m_triggerReset)
				return false;

			if (m_slide != null && m_slide.isSelected)
				return false;

			if (!BulletInChamber)
				return false;

			if (Data.shootType == WeaponData.ShootType.Automatic && m_fireRateTimer < 60.0f / Data.roundsPerMinute)
				return false;

			return true;
		}

		public virtual void TryToCock() {
			if (BulletInChamber) {
				EjectCartridge(true);
				BulletInChamber = false;
			}

			if (!m_magSlot.Mag || m_magSlot.Mag.bulletCount == 0)
				return;

			m_magSlot.Mag.bulletCount--;
			BulletInChamber = true;

			OnCocked();
		}

		protected virtual void OnCocked() {
		}

		// This is triggered from animation
		public void EjectEmptyCartridge() => EjectCartridge();

		public virtual void EjectCartridge(bool withBullet = false) {
			Cartridge cartridge = Instantiate(Data.cartridgeData.cartridgePrefab, m_cartridgeEjectPoint.position, m_cartridgeEjectPoint.rotation);
			float force = Random.Range(0.7f, 1.4f);
			cartridge.Eject(withBullet, force);
		}

		protected virtual void UpdateTriggerValue(float normalizedTriggerValue) {
			if (normalizedTriggerValue >= m_data.fireTriggerValue) {
				if (CanFire())
					Fire();
				else if (m_triggerReset)
					DryFire();
			}
			if (!m_triggerReset && normalizedTriggerValue < m_data.resetTriggerValue)
				m_triggerReset = true;

			if (m_visualTriggerTransform != null) {
				float visualTriggerRotation = Mathf.Lerp(m_data.triggerRotationRange.min, m_data.triggerRotationRange.max, normalizedTriggerValue);
				m_visualTriggerTransform.localEulerAngles = new Vector3(visualTriggerRotation, 0, 0);
			}
		}

		protected virtual void FireProjectile() {
			if (Data.shootingPhysicsType != WeaponData.ShootingPhysicsType.Projectile) {
				Debug.LogWarning("Tried to fire projectile from a weapon that does not have projectile bullet physics");
				return;
			}

			Bullet bullet = GameObject.Instantiate<Bullet>(m_data.cartridgeData.bulletPrefab, m_barrelEndPoint.position, m_barrelEndPoint.rotation);

			foreach (Collider collider in colliders)
				Physics.IgnoreCollision(collider, bullet.Collider);

			bullet.Fire(m_data);
		}

		protected virtual void OnReleaseMagPressed(InputAction.CallbackContext context) {
			m_magSlot.ReleaseMagazine();
		}

		protected virtual void OnSlideStopPressed(InputAction.CallbackContext context) {
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (GripHand != null && (IXRSelectInteractor)GripHand != interactor)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			GripHand = (Hand)args.interactorObject;
			GripHand.MagReleaseAction.performed += OnReleaseMagPressed;
			GripHand.SlideReleaseAction.performed += OnSlideStopPressed;
			m_fireRateTimer = 60.0f / Data.roundsPerMinute;
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);
			UpdateTriggerValue(0);
			GripHand.MagReleaseAction.performed -= OnReleaseMagPressed;
			GripHand.SlideReleaseAction.performed -= OnSlideStopPressed;
			GripHand = null;
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (GripHand != null && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Late)
				UpdateTriggerValue(GripHand.TriggerAction.ReadValue<float>());
		}
	}
}
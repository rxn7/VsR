using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public abstract class WeaponBase : XRGrabInteractable {
		[SerializeField] private WeaponData m_data;
		[SerializeField] private MagazineSlot m_magSlot;
		[SerializeField] private WeaponSlide m_slide;
		[SerializeField] private Transform m_visualTriggerTransform;
		[SerializeField] private GameObject m_cartridgeInChamber;
		[SerializeField] private Transform m_barrelEndPoint;
		[SerializeField] private Transform m_cartridgeEjectPoint;

		protected Animator m_animator;
		protected Hand m_gripHand = null;
		protected Rigidbody m_rb;
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
		public Animator Animator => m_animator;
		public Hand GripHand => m_gripHand;

		protected override void Awake() {
			base.Awake();

			movementType = MovementType.Instantaneous;
			m_magSlot.weapon = this;
			m_slide.weapon = this;

			m_animator = GetComponent<Animator>();
			m_rb = GetComponent<Rigidbody>();

			SetTriggerValue(0);
			BulletInChamber = false;
		}

		protected virtual void Update() {
			if (!isSelected)
				return;

			m_fireRateTimer += Time.deltaTime;
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmos() {
			if (m_barrelEndPoint != null) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(m_barrelEndPoint.position, 0.01f);
			}

			if (m_cartridgeEjectPoint != null) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(m_cartridgeEjectPoint.position, m_cartridgeEjectPoint.position + m_cartridgeEjectPoint.up * 0.1f);
			}
		}
#endif

		protected virtual void DryFire() {
			SoundManager.Instance.PlaySound(m_data.dryFireSound, transform.position, Random.Range(0.9f, 1.1f));
			m_triggerReset = false;
		}

		protected virtual void Fire() {
			BulletInChamber = false;
			m_triggerReset = false;
			m_fireRateTimer = 0.0f;

			m_animator.SetTrigger("Shoot");
			m_gripHand.ApplyHapticFeedback(m_data.fireHapticFeedback);
			m_gripHand.Recoil.AddRecoil(m_data.recoilInfo);
			SoundManager.Instance.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));
			Invoke("EjectEmptyCartridge", m_data.ejectCartridgeDelaySec);

			FireProjectile();

			// Automatic & SemiAutomatic weapons' slide moves back from recoil
			if (m_data.shootType != WeaponData.ShootType.Manual)
				TryToCock();
		}

		protected virtual bool CanFire() {
			if (m_data.shootType != WeaponData.ShootType.Automatic && !m_triggerReset)
				return false;

			if (m_slide != null && m_slide.isSelected)
				return false;

			if (!BulletInChamber)
				return false;

			if (m_data.shootType == WeaponData.ShootType.Automatic && m_fireRateTimer < m_data.SecondsPerRound)
				return false;

			return true;
		}

		public virtual void TryToCock() {
			if (BulletInChamber) {
				EjectCartridge(true);
				BulletInChamber = false;
			}

			if (!m_magSlot.Mag || m_magSlot.Mag.IsEmpty)
				return;

			m_magSlot.Mag.bulletCount--;
			BulletInChamber = true;

			OnCocked();
		}

		protected virtual void OnCocked() {
		}

		protected void EjectEmptyCartridge() => EjectCartridge(false);
		protected virtual void EjectCartridge(bool withBullet = false) {
			Cartridge cartridge = Instantiate(m_data.cartridgeData.cartridgePrefab, m_cartridgeEjectPoint.position, m_cartridgeEjectPoint.rotation);
			float force = Random.Range(0.7f, 1.4f);
			cartridge.Eject(withBullet, force);
		}

		protected virtual void SetTriggerValue(float normalizedTriggerValue) {
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
			if (m_data.shootingPhysicsType != WeaponData.ShootingPhysicsType.Projectile) {
				Debug.LogWarning("Tried to fire projectile from a weapon that does not have projectile bullet physics");
				return;
			}

			Bullet bullet = GameObject.Instantiate<Bullet>(m_data.cartridgeData.bulletPrefab, m_barrelEndPoint.position, m_barrelEndPoint.rotation);

			foreach (Collider collider in colliders)
				Physics.IgnoreCollision(collider, bullet.Collider);

			bullet.Fire(m_data);
		}

		protected virtual void OnGripHandAttached(Hand hand) {
			m_gripHand = hand;
			m_gripHand.MagReleaseAction.performed += OnReleaseMagPressed;
			m_gripHand.SlideReleaseAction.performed += OnSlideStopPressed;
			m_fireRateTimer = m_data.SecondsPerRound;
		}

		protected virtual void OnGripHandDetached() {
			SetTriggerValue(0);
			if (m_gripHand != null) {
				m_gripHand.MagReleaseAction.performed -= OnReleaseMagPressed;
				m_gripHand.SlideReleaseAction.performed -= OnSlideStopPressed;
			}
			m_gripHand = null;
		}

		protected virtual void OnReleaseMagPressed(InputAction.CallbackContext context) {
			m_magSlot.ReleaseMagazine();
		}

		protected virtual void OnSlideStopPressed(InputAction.CallbackContext context) {
		}

		protected virtual void UpdateTrigger() {
			SetTriggerValue(m_gripHand.TriggerAction.ReadValue<float>());
		}

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (isSelected && !IsSelected(interactor))
				return false;

			return base.IsSelectableBy(interactor);
		}

		public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
			base.ProcessInteractable(updatePhase);

			if (isSelected && updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic) {
				UpdateTrigger();
			}
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);
			OnGripHandAttached(args.interactorObject as Hand);
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);
			OnGripHandDetached();
		}
	}
}
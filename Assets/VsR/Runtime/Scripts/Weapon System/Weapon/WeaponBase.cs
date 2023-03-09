using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public abstract class WeaponBase : XRGrabInteractable {
		public delegate void OnFire();
		public event OnFire onFire;

		[SerializeField] protected WeaponData m_data;
		[SerializeField] protected MagazineSlot m_magSlot;
		[SerializeField] protected WeaponTrigger m_trigger;
		[SerializeField] protected GameObject m_cartridgeInChamberObject;
		[SerializeField] protected Transform m_barrelEndPoint;
		[SerializeField] protected Transform m_cartridgeEjectPoint;

		protected Hand m_gripHand = null;
		private float m_fireRateTimer = 0.0f;
		private Vector3 m_previousPosition;
		private bool m_triggerReset = true;
		private bool _m_cartridgeInChamber;
		private Vector3 m_velocity;

		public bool CartridgeInChamber {
			get => _m_cartridgeInChamber;
			set {
				_m_cartridgeInChamber = value;
				m_cartridgeInChamberObject.SetActive(value);
			}
		}
		public WeaponData Data => m_data;
		public Hand GripHand => m_gripHand;
		public Vector3 WorldVelocity => m_velocity;
		public Transform CartridgeEjectPoint => m_cartridgeEjectPoint;

		protected override void Awake() {
			base.Awake();

			movementType = MovementType.Instantaneous;

			SetTriggerValue(0);
			CartridgeInChamber = false;
		}

		protected virtual void Update() {
			if (!isSelected)
				return;

			m_velocity = (transform.position - m_previousPosition) / Time.unscaledDeltaTime;
			m_previousPosition = transform.position;

			m_fireRateTimer += Time.deltaTime;
			UpdateTrigger();
		}

		protected virtual void OnDrawGizmos() {
			if (m_barrelEndPoint) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(m_barrelEndPoint.position, 0.01f);
			}

			if (m_cartridgeEjectPoint) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(m_cartridgeEjectPoint.position, m_cartridgeEjectPoint.position + m_cartridgeEjectPoint.up * 0.1f);
			}
		}

		protected virtual void DryFire() {
			SoundPoolManager.Instance.PlaySound(m_data.dryFireSound, transform.position, Random.Range(0.9f, 1.1f));
			m_triggerReset = false;
		}

		protected virtual void Fire() {
			CartridgeInChamber = false;
			m_triggerReset = false;
			m_fireRateTimer = 0.0f;

			m_gripHand.ApplyHapticFeedback(m_data.fireHapticFeedback);
			m_gripHand.Recoil.AddRecoil(m_data.recoilInfo);
			SoundPoolManager.Instance.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));

			if (m_data.shootType != WeaponData.ShootType.Manual)
				TryToCock();

			switch (Data.shootingPhysicsType) {
				case WeaponData.ShootingPhysicsType.RaycastLaser:
					ShootingPhysics.Instance.LaserRaycast(m_barrelEndPoint, m_data);
					break;
			}

			onFire?.Invoke();
		}

		protected virtual bool CanFire() {
			if (m_data.shootType != WeaponData.ShootType.Automatic && !m_triggerReset)
				return false;

			if (!CartridgeInChamber)
				return false;

			if (m_fireRateTimer < m_data.SecondsPerRound)
				return false;

			return true;
		}

		public virtual void TryToCock() {
			if (CartridgeInChamber) {
				EjectCartridge(true);
				CartridgeInChamber = false;
			}

			if (!m_magSlot.Mag || m_magSlot.Mag.IsEmpty)
				return;

			m_magSlot.Mag.bulletCount--;
			CartridgeInChamber = true;

			OnCocked();
		}

		protected virtual void OnCocked() {
		}

		private void EjectEmptyCartridge() => EjectCartridge(false);
		public virtual void EjectCartridge(bool withBullet = false) {
			Cartridge cartridge = CartridgePoolManager.Instance.Pool.Get();
			cartridge.Eject(this, withBullet);
		}

		protected virtual void SetTriggerValue(float normalizedTriggerValue) {
			if (normalizedTriggerValue >= m_data.fireTriggerPressure) {
				if (CanFire())
					Fire();
				else if (m_triggerReset)
					DryFire();
			}

			if (!m_triggerReset && normalizedTriggerValue < m_data.resetTriggerPressure) {
				m_triggerReset = true;
				GripHand.SendHapticImpulse(0.1f, 0.1f);
			}

			m_trigger.UpdateRotation(normalizedTriggerValue);
		}

		protected virtual void OnGripHandAttached(Hand hand) {
			m_gripHand = hand;
			m_gripHand.MagReleaseAction.performed += OnReleaseMagPressed;
			m_gripHand.SlideReleaseAction.performed += OnSlideReleasePressed;
			m_fireRateTimer = m_data.SecondsPerRound;
		}

		protected virtual void OnGripHandDetached() {
			SetTriggerValue(0);
			if (m_gripHand) {
				m_gripHand.MagReleaseAction.performed -= OnReleaseMagPressed;
				m_gripHand.SlideReleaseAction.performed -= OnSlideReleasePressed;
			}
			m_gripHand = null;
		}

		protected virtual void OnReleaseMagPressed(InputAction.CallbackContext context) {
			m_magSlot.ReleaseMagazine();
		}

		protected virtual void OnSlideReleasePressed(InputAction.CallbackContext context) { }

		protected virtual void UpdateTrigger() => SetTriggerValue(m_gripHand.TriggerAction.ReadValue<float>());

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			if (interactor is not Hand)
				return false;

			if (isSelected && !interactorsSelecting.Contains(interactor))
				return false;

			return base.IsSelectableBy(interactor);
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
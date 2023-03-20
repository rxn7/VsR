using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	public class Weapon : XRGrabInteractable {
		public event System.Action onFire;

		[SerializeField] protected WeaponData m_data;
		[SerializeField] protected WeaponMagazineSlot m_magSlot;
		[SerializeField] protected WeaponTrigger m_trigger;
		[SerializeField] protected GameObject m_cartridgeInChamberObject;
		[SerializeField] protected Transform m_barrelEndPoint;
		[SerializeField] protected Transform m_cartridgeEjectPoint;

		protected Hand m_gripHand = null;
		protected Hand m_guardHand = null;
		protected WeaponGuardHold m_guardHold = null;
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
		public Hand GuardHand => m_guardHand;
		public WeaponGuardHold HeldGuardHold => m_guardHold;
		public Vector3 WorldVelocity => m_velocity;
		public Transform CartridgeEjectPoint => m_cartridgeEjectPoint;

		protected override void Awake() {
			base.Awake();

			foreach (WeaponGuardHold hold in GetComponentsInChildren<WeaponGuardHold>()) {
				hold.selectEntered.AddListener(OnGuardHandAttached);
				hold.selectExited.AddListener(OnGuardHandDetached);
			}

			movementType = MovementType.Instantaneous;
			interactionLayers = InteractionLayerMask.GetMask("Storable", "Hand");

			SetTriggerValue(0);
			CartridgeInChamber = false;
		}

		protected void Update() {
			if (!m_gripHand)
				return;

			m_velocity = (transform.position - m_previousPosition) / Time.unscaledDeltaTime;
			m_previousPosition = transform.position;

			m_fireRateTimer += Time.deltaTime;
			SetTriggerValue(m_gripHand.TriggerAction.ReadValue<float>());
		}

		protected void OnDrawGizmos() {
			if (m_barrelEndPoint) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(m_barrelEndPoint.position, 0.004f);
			}

			if (m_cartridgeEjectPoint) {
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(m_cartridgeEjectPoint.position, m_cartridgeEjectPoint.position + m_cartridgeEjectPoint.up * 0.1f);
			}
		}

		protected void Fire() {
			CartridgeInChamber = false;
			m_triggerReset = false;
			m_fireRateTimer = 0.0f;

			m_gripHand.ApplyHapticFeedback(m_data.fireHapticFeedback);
			m_gripHand.Recoil.AddRecoil(this);
			SoundPoolManager.Instance.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));

			if (m_data.shootType != WeaponData.ShootType.Manual)
				TryToCock();

			switch (Data.shootingPhysicsType) {
				case WeaponData.ShootingPhysicsType.RaycastLaser:
					ShootingPhysics.LaserRaycast(m_barrelEndPoint, m_data);
					break;
			}

			onFire?.Invoke();
		}


		protected void SetTriggerValue(float normalizedTriggerValue) {
			if (normalizedTriggerValue >= m_data.fireTriggerPressure) {
				if (CanFire())
					Fire();
				else if (m_triggerReset)
					m_triggerReset = false;
			}

			if (!m_triggerReset && normalizedTriggerValue < m_data.resetTriggerPressure)
				m_triggerReset = true;

			m_trigger.UpdateRotation(normalizedTriggerValue);
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

		public void TryToCock() {
			if (CartridgeInChamber) {
				EjectCartridge(true);
				CartridgeInChamber = false;
			}

			if (!m_magSlot.Mag || m_magSlot.Mag.IsEmpty)
				return;

			m_magSlot.Mag.bulletCount--;
			CartridgeInChamber = true;
		}

		public void EjectCartridge(bool withBullet = false) {
			float force, torque;
			Math.FloatRange randomness;

			// If the cartridge has a bullet, we can assume it was ejected by manually pulling the slide instead of firing
			if (!withBullet) {
				force = m_data.cartridgeEjectForce;
				torque = m_data.cartridgeEjectTorque;
				randomness = new Math.FloatRange(0.5f, 1.5f);
			} else {
				force = 1.5f;
				torque = 5.0f;
				randomness = new Math.FloatRange(0.9f, 1.1f);
			}

			Cartridge cartridge = CartridgePoolManager.Instance.Pool.Get();
			cartridge.Eject(this, force, torque, randomness, withBullet);
		}

		protected void OnGripHandAttached(Hand hand) {
			m_gripHand = hand;
			m_gripHand.MagReleaseAction.performed += OnReleaseMagPressed;
			m_gripHand.SlideReleaseAction.performed += OnSlideReleasePressed;
			m_fireRateTimer = m_data.SecondsPerRound;
		}

		protected void OnGripHandDetached() {
			SetTriggerValue(0);
			if (m_gripHand) {
				m_gripHand.MagReleaseAction.performed -= OnReleaseMagPressed;
				m_gripHand.SlideReleaseAction.performed -= OnSlideReleasePressed;
			}
			m_gripHand = null;
		}

		protected void OnGuardHandAttached(SelectEnterEventArgs args) {
			m_guardHold = (WeaponGuardHold)args.interactableObject;
			m_guardHand = (Hand)args.interactorObject;
		}

		protected void OnGuardHandDetached(SelectExitEventArgs args) {
			m_guardHand = null;
			m_guardHold = null;
		}

		protected virtual void OnReleaseMagPressed(InputAction.CallbackContext context) {
			m_magSlot.ReleaseMagazine();
		}

		protected virtual void OnSlideReleasePressed(InputAction.CallbackContext context) { }

		public override bool IsSelectableBy(IXRSelectInteractor interactor) {
			// Nothing can select the weapon if it's held by a grip hand
			if (m_gripHand && interactor != (IXRSelectInteractor)m_gripHand)
				return false;

			return base.IsSelectableBy(interactor);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs args) {
			base.OnSelectEntered(args);

			if (args.interactorObject is Hand hand)
				OnGripHandAttached(hand);
		}

		protected override void OnSelectExited(SelectExitEventArgs args) {
			base.OnSelectExited(args);

			if (args.interactorObject is Hand)
				OnGripHandDetached();
		}
	}
}
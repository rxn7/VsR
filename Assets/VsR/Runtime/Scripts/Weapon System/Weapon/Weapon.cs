using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace VsR {
    [RequireComponent(typeof(VelocityTracker))]
	public class Weapon : XRGrabInteractable {
		public event System.Action onFire;
        public VelocityTracker VelocityTracker { get; private set; }

		[Required] [SerializeField] protected WeaponData m_data;
		[Required] [SerializeField] protected WeaponMagazineSlot m_magSlot;
		[Required] [SerializeField] protected WeaponTrigger m_trigger;
		[Required] [SerializeField] protected Transform m_chamberedCartridgeSlot;
		[Required] [SerializeField] protected Transform m_barrelEndPoint;

		protected Hand m_gripHand = null;
		protected Hand m_guardHand = null;
		protected WeaponGuardHold m_guardHold = null;
		private float m_fireRateTimer = 0.0f;
		private bool m_triggerReset = true;
		private Cartridge _m_chamberedCartridge;

        public Cartridge chamberedCartridge {
            get => _m_chamberedCartridge;
            set {
                _m_chamberedCartridge = value;
                if(value != null) {
                    _m_chamberedCartridge.transform.SetParent(m_chamberedCartridgeSlot, true);
                    _m_chamberedCartridge.transform.localPosition = Vector3.zero;
                    _m_chamberedCartridge.transform.localEulerAngles = Vector3.zero;
                }
            }
        }

		public WeaponData Data => m_data;
		public Hand GripHand => m_gripHand;
		public Hand GuardHand => m_guardHand;
		public WeaponGuardHold HeldGuardHold => m_guardHold;

		protected override void Awake() {
			base.Awake();

			foreach (WeaponGuardHold hold in GetComponentsInChildren<WeaponGuardHold>()) {
				hold.selectEntered.AddListener(OnGuardHandAttached);
				hold.selectExited.AddListener(OnGuardHandDetached);
			}

            VelocityTracker = GetComponent<VelocityTracker>();
			movementType = MovementType.Instantaneous;
			interactionLayers = InteractionLayerMask.GetMask("Storable", "Hand");

			SetTriggerValue(0);
			chamberedCartridge = null;
		}

		protected void Update() {
			if (m_gripHand == null)
				return;

			m_fireRateTimer += Time.deltaTime;
			SetTriggerValue(m_gripHand.TriggerAction.ReadValue<float>());
		}

		protected void OnDrawGizmos() {
			if (m_barrelEndPoint) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(m_barrelEndPoint.position, 0.004f);
			}
		}

		protected void Fire() {
			chamberedCartridge.hasBullet = false;
			m_triggerReset = false;
			m_fireRateTimer = 0.0f;

			m_gripHand.ApplyHapticFeedback(m_data.fireHapticFeedback);
			m_gripHand.Recoil.AddRecoil(this);
			SoundPoolManager.Instance.PlaySound(m_data.shootSound, transform.position, Random.Range(0.9f, 1.1f));

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

			if (!chamberedCartridge)
				return false;

			if (m_fireRateTimer < m_data.SecondsPerRound)
				return false;

			return true;
		}

		public void Rack() {
			if (chamberedCartridge != null || m_magSlot.Mag == null || m_magSlot.Mag.IsEmpty)
				return;

			chamberedCartridge = m_magSlot.Mag.PopCartridge();
		}

		public void EjectChamberedCartridge(bool manually) {
            if(chamberedCartridge == null)
                return;

			float force, torque;
			Math.FloatRange randomness;

			if (manually) {
				force = 1.5f;
				torque = 5.0f;
				randomness = new Math.FloatRange(0.9f, 1.1f);
			} else {
				force = m_data.cartridgeEjectForce;
				torque = m_data.cartridgeEjectTorque;
				randomness = new Math.FloatRange(0.5f, 1.5f);
			}

			chamberedCartridge.Eject(VelocityTracker.velocity, chamberedCartridge.transform, force, torque, randomness);
            chamberedCartridge = null;
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
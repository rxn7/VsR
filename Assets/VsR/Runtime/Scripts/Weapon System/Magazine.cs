using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TriInspector;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.InputSystem.InputAction;

namespace VsR {
    [RequireComponent(typeof(VelocityTracker))]
	public class Magazine : XRGrabInteractable {
		public const float EMPTY_MAG_DROP_DESTROY_DELAY_SECS = 10.0f;

		[Header("Magazine")]
		[Required] [SerializeField] private MagazineData m_data;

        private VelocityTracker m_velocityTracker;
        private Transform m_cartridgeContainer;
		private Stack<Cartridge> m_cartridges = new();
		private Rigidbody m_rb;

		public int CartridgeCount => m_cartridges.Count;
		public bool IsEmpty => CartridgeCount == 0;
		public MagazineData Data => m_data;

		protected override void Awake() {
            base.Awake();

			if (!m_data) {
				Destroy(gameObject);
				throw new UnassignedReferenceException($"m_data is not assigned on {nameof(Magazine)}!");
			}

            m_cartridgeContainer = new GameObject("Cartridge Container").transform;
            m_cartridgeContainer.SetParent(transform);

            m_velocityTracker = GetComponent<VelocityTracker>();
			m_rb = GetComponent<Rigidbody>();
			m_rb.interpolation = RigidbodyInterpolation.Interpolate;
			m_rb.collisionDetectionMode = CollisionDetectionMode.Discrete;

            selectEntered.AddListener(OnSelectEnter);
            selectExited.AddListener(OnSelectExit);

            SpawnCartridges();
		}

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            m_data.CalculateCartridgeSlots();
            Gizmos.matrix = Matrix4x4.TRS(transform.localPosition, transform.rotation, Vector3.one);
            for(int i=0; i<m_data.calculatedCartridgeSlots.Length; ++i) {
                Gizmos.color = i % 2 == 0 ? Color.yellow : Color.green;
                MagazineCartridgeSlot slot = m_data.calculatedCartridgeSlots[i];
                Quaternion rotation = Quaternion.Euler(slot.rotation);
                Gizmos.DrawMesh(m_data.defaultCartridgeData.bulletMesh, 0, slot.position, rotation);
                Gizmos.DrawMesh(m_data.defaultCartridgeData.cartridgeMesh, 0, slot.position, rotation);

                if(i < m_data.calculatedCartridgeSlots.Length - 1) { 
                    Gizmos.DrawLine(slot.position, m_data.calculatedCartridgeSlots[i+1].position);
                }
            }
        }
#endif

		public void SlideIn() {
			m_rb.isKinematic = true;
			m_rb.detectCollisions = false;
		}

		public void SlideOut(Vector3 weaponVelocity) {
			m_rb.isKinematic = false;
			m_rb.detectCollisions = true;
			m_rb.velocity = -transform.up * 0.3f + weaponVelocity;
		}

        public Cartridge PopCartridge(bool updateSlots = true) {
            if(CartridgeCount == 0) {
                return null;
            }

            Cartridge cartridge = m_cartridges.Pop();
            cartridge.transform.SetParent(null, true);

            if(updateSlots)
                UpdateCartridgePositions();

            return cartridge;
        }

        public void PushCartridge(CartridgeData data, bool updateSlots = true) {
            if(m_cartridges.Count == m_data.capacity)
                throw new Exception("Can't fit any more cartridges in this magazine!");

            Cartridge cartridge = data.Spawn(m_cartridgeContainer, true);
            m_cartridges.Push(cartridge);

            if(updateSlots)
                UpdateCartridgePositions();
        }

        [ContextMenu("Spawn cartridges")]
        private void SpawnCartridges() {
            m_cartridgeContainer = new GameObject("Cartridge Container").transform;
            m_cartridgeContainer.SetParent(transform);

            m_cartridges.Clear();
            while(m_cartridgeContainer.childCount > 0) {
                DestroyImmediate(m_cartridgeContainer.GetChild(0).gameObject);
            }

            if(m_data?.defaultCartridgeData == null) 
                return;

            for(int i=0; i<m_data.capacity; ++i) {
                PushCartridge(m_data.defaultCartridgeData, false);
            }

            UpdateCartridgePositions();
        }

        private void UpdateCartridgePositions() {
            int i=0;
            foreach(Cartridge cartridge in m_cartridges) {
                MagazineCartridgeSlot slot = m_data.calculatedCartridgeSlots[i++];
                cartridge.transform.localPosition = slot.position;
                cartridge.transform.localEulerAngles = slot.rotation;
            }
        }

		protected override void Drop() {
			base.Drop();

			if (IsEmpty)
				Destroy(gameObject, EMPTY_MAG_DROP_DESTROY_DELAY_SECS);
		}

        private void OnMagPopCartridge(CallbackContext ctx) {
            Cartridge cartridge = PopCartridge();
            if(cartridge == null)
                return;

            cartridge.Eject(m_velocityTracker.velocity, cartridge.transform, 1.0f, 1.0f, new Math.FloatRange(0.9f, 1.1f));
        }

        private void OnSelectEnter(SelectEnterEventArgs args) {
            if(args.interactorObject is Hand hand) {
                hand.MagPopCartridgeAction.performed += OnMagPopCartridge;
            }
        }

        private void OnSelectExit(SelectExitEventArgs args) {
            if(args.interactorObject is Hand hand) {
                hand.MagPopCartridgeAction.performed -= OnMagPopCartridge;
            }
        }
	}
}

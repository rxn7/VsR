using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	[RequireComponent(typeof(LineRenderer))]
	public class HandInteractionRay : MonoBehaviour {
		[SerializeField] private Hand m_hand;
		[SerializeField] private float m_maxDistance = 5;
		[SerializeField] private LayerMask m_rayLayerMask;

		[Header("Haptic Feedback")]
		[SerializeField] private HapticFeedback m_hoverEnterHapticFeedback;
		[SerializeField] private HapticFeedback m_hoverExitHapticFeedback;

		private InputAction m_enableRaycastAction;
		private LineRenderer m_lineRenderer;
		private XRBaseInteractable m_hoveringInteractable = null;
		private Ray m_ray;
		private bool m_wasHoveringLastFrame = false;

		public bool IsHovering => m_hoveringInteractable != null;

		private void Awake() {
			m_lineRenderer = GetComponent<LineRenderer>();
			m_ray = new Ray();
		}

		private void Start() {
			m_enableRaycastAction = InputActionManager.Instance.GetInteractionAction(m_hand.HandType, "Enable Raycast");
		}

		private void LateUpdate() {
			if (!m_enableRaycastAction.IsPressed() || m_hand.interactablesSelected.Count > 0) {
				m_lineRenderer.enabled = false;
				m_hoveringInteractable = null;
				return;
			}

			m_lineRenderer.enabled = true;
			transform.SetPositionAndRotation(m_hand.attachTransform.position, m_hand.attachTransform.rotation);

			PerformRaycast();

			if (m_hand.GrabAction.WasPressedThisFrame())
				OnGrab();

			if (!m_wasHoveringLastFrame && IsHovering)
				m_hand.ApplyHapticFeedback(m_hoverEnterHapticFeedback);
			else if (m_wasHoveringLastFrame && !IsHovering && m_hand.interactablesSelected.Count == 0)
				m_hand.ApplyHapticFeedback(m_hoverExitHapticFeedback);

			m_wasHoveringLastFrame = IsHovering;
		}

		private void PerformRaycast() {
			m_ray.origin = transform.position;
			m_ray.direction = transform.forward;

			Color color = Color.clear;
			Vector3 rayEndPoint = Vector3.zero;

			if (Physics.Raycast(m_ray, out RaycastHit hit, m_maxDistance, m_rayLayerMask)) {
				rayEndPoint = hit.point;
				color = Color.green;

				if (hit.transform.TryGetComponent<XRBaseInteractable>(out XRBaseInteractable interactable)) {
					m_hoveringInteractable = interactable;
				} else {
					m_hoveringInteractable = null;
				}
			} else {
				m_hoveringInteractable = null;
			}

			if (!IsHovering) {
				m_hoveringInteractable = null;
				rayEndPoint = transform.position + m_ray.direction * m_maxDistance;
				color = Color.red;
			}

			m_lineRenderer.material.color = color;

			m_lineRenderer.SetPosition(0, m_hand.attachTransform.position);
			m_lineRenderer.SetPosition(1, rayEndPoint);
		}

		private void OnGrab() {
			if (!IsHovering || m_hand.interactablesSelected.Count > 0)
				return;

			m_hand.interactionManager.SelectEnter(m_hand, (IXRSelectInteractable)m_hoveringInteractable);
			m_hoveringInteractable = null;
		}
	}
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace VsR {
	[RequireComponent(typeof(LineRenderer))]
	public class HandInteractionRay : MonoBehaviour {
		[SerializeField] private Hand m_hand;
		[SerializeField] private float m_maxDistance = 50;
		[SerializeField] private LayerMask m_rayLayerMask;
		private InputAction m_enableRaycastAction;
		private LineRenderer m_lineRenderer;
		private Ray m_ray;

		private void Awake() {
			m_lineRenderer = GetComponent<LineRenderer>();
			m_ray = new Ray();
		}

		private void Start() {
			m_enableRaycastAction = InputActionManager.Instance.GetInteractionAction(m_hand.HandType, "Enable Raycast");
		}

		private void LateUpdate() {
			if (!m_enableRaycastAction.IsPressed()) {
				m_lineRenderer.enabled = false;
				return;
			}

			m_lineRenderer.enabled = true;
			transform.SetPositionAndRotation(m_hand.attachTransform.position, m_hand.attachTransform.rotation);

			m_ray.origin = transform.position;
			m_ray.direction = transform.forward;

			Vector3 rayEndPoint;
			if (Physics.Raycast(m_ray, out RaycastHit hit, m_maxDistance, m_rayLayerMask)) {
				rayEndPoint = hit.point;
			} else {
				rayEndPoint = transform.position + m_ray.direction * m_maxDistance;
			}

			m_lineRenderer.SetPosition(0, m_hand.attachTransform.position);
			m_lineRenderer.SetPosition(1, rayEndPoint);
		}
	}
}

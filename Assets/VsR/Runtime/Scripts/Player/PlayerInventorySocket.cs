using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VsR {
	[ExecuteAlways]
	public class PlayerInventorySocket : XRSocketInteractor {
		[SerializeField] private Transform m_head;
		[SerializeField] private float m_heightRatio;
		private XRBaseInteractable m_trackedItem;

		private void Update() {
			transform.position = new Vector3(transform.position.x, m_head.position.y * m_heightRatio, transform.position.z);
		}

		private void OnTrackItemSelectExited(SelectExitEventArgs args) {
		}

		protected override void OnHoverEntered(HoverEnterEventArgs args) {
			base.OnHoverEntered(args);
			m_trackedItem = (XRBaseInteractable)args.interactableObject;
			m_trackedItem.selectExited.AddListener(OnTrackItemSelectExited);
		}

		protected override void OnHoverExited(HoverExitEventArgs args) {
			base.OnHoverExited(args);
			m_trackedItem?.selectExited.RemoveListener(OnTrackItemSelectExited);
			m_trackedItem = null;
		}

		public override bool CanSelect(IXRSelectInteractable interactable) {
			if (!IsSelecting(interactable) && !interactable.interactorsSelecting.Any(i => i is HandInteractor))
				return false;

			return base.CanSelect(interactable);
		}
	}
}
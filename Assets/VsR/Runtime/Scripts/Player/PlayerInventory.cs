using UnityEngine;

namespace VsR {
	[ExecuteAlways]
	public class PlayerInventory : MonoBehaviour {
		[SerializeField] private Transform m_head;
		[SerializeField] private float m_rotationFollowSpeed = 20;

		private void Update() {
			transform.position = m_head.position;

			Quaternion targetRotation = new Quaternion(transform.rotation.x, m_head.rotation.y, transform.rotation.z, m_head.rotation.w);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_rotationFollowSpeed * Time.deltaTime);
		}
	}
}

using UnityEngine;

namespace VsR {
	[System.Serializable]
	public struct PlayerInventorySocketData {
		public Transform transform;
		[Range(0.0f, 1.0f)] public float heightRatio;
	}

	[ExecuteAlways]
	public class PlayerInventory : MonoBehaviour {
		[SerializeField] private Transform m_head;
		[SerializeField] private PlayerInventorySocketData[] m_sockets;

		private void Update() {
			transform.position = m_head.position;
			transform.rotation = new Quaternion(transform.rotation.x, m_head.rotation.y, transform.rotation.z, m_head.rotation.w);

			foreach (PlayerInventorySocketData socketData in m_sockets)
				socketData.transform.position = new Vector3(socketData.transform.position.x, m_head.position.y * socketData.heightRatio, socketData.transform.position.z);
		}
	}
}

using UnityEngine;

namespace VsR {
	public class SpawnMenu : MonoBehaviour {
		[SerializeField] private Transform m_itemContainer;
		[SerializeField] private SpawnMenuItem m_itemPrefab;
		public Transform m_spawnPoint;

		private void Awake() {
			if (!m_spawnPoint)
				m_spawnPoint = transform;

			Item[] items = Resources.LoadAll<Item>("");

			foreach (Item item in items) {
				SpawnMenuItem menuItem = Instantiate(m_itemPrefab, m_itemContainer);
				menuItem.Item = item;
				menuItem.m_menu = this;
			}
		}
	}
}

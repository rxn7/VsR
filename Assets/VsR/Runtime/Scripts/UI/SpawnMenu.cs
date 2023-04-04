using UnityEngine;

namespace VsR {
	public class SpawnMenu : MonoBehaviour {
		[SerializeField] private Transform m_itemContainer;
		[SerializeField] private SpawnMenuItem m_itemPrefab;

		private void Awake() {
			Item[] items = Resources.LoadAll<Item>("");

			foreach (Item item in items) {
				SpawnMenuItem menuItem = Instantiate(m_itemPrefab, m_itemContainer);
				menuItem.Item = item;
			}
		}
	}
}

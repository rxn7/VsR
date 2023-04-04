using UnityEngine;
using UnityEngine.UI;

namespace VsR {
	public class SpawnMenuItem : MonoBehaviour {
		private Button m_btn;
		private TMPro.TextMeshProUGUI m_btnText;
		private Item _m_item;

		public Item Item {
			get => _m_item;
			set {
				_m_item = value;
				m_btnText.text = _m_item.displayName;
			}
		}

		private void Awake() {
			m_btn = GetComponent<Button>();
			m_btnText = m_btn.GetComponentInChildren<TMPro.TextMeshProUGUI>();

			m_btn.onClick.AddListener(OnClick);
		}

		private void OnClick() {
			GameObject obj = Instantiate(Item.prefab, transform.position + Vector3.up, Quaternion.identity);
		}
	}
}
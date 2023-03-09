using UnityEngine;
using UnityEngine.Pool;

namespace VsR {
	public class CartridgePoolManager : SingletonBehaviour<CartridgePoolManager> {
		private GameObject m_prefab;
		public ObjectPool<Cartridge> Pool { get; private set; }

		protected override void Awake() {
			base.Awake();
			m_prefab = (GameObject)Resources.Load("Prefabs/Cartridge");
			Pool = new ObjectPool<Cartridge>(CreatePooledCartridge, (Cartridge b) => b.OnGet(), (Cartridge b) => b.OnRelease(), (Cartridge b) => Destroy(b), true, 30, 100);
		}

		private Cartridge CreatePooledCartridge() {
			Cartridge b = Instantiate(m_prefab, transform, false).GetComponent<Cartridge>();
			b.gameObject.SetActive(false);
			return b;
		}
	}
}
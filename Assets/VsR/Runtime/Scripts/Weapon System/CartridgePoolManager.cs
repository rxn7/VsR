using UnityEngine;
using UnityEngine.Pool;

namespace VsR {
	public static class CartridgePoolManager {
		private const string PREFAB_PATH = "Prefabs/Pooled/Cartridge";
		private static GameObject s_prefab;
		public static ObjectPool<Cartridge> Pool { get; private set; }

		[RuntimeInitializeOnLoadMethod()]
		private static void Init() {
			s_prefab = (GameObject)Resources.Load(PREFAB_PATH);
			Pool = new ObjectPool<Cartridge>(CreatePooledCartridge, (Cartridge b) => b.OnGet(), (Cartridge b) => b.OnRelease(), (Cartridge b) => GameObject.Destroy(b), true, 30, 100);
		}

		private static Cartridge CreatePooledCartridge() {
			Cartridge b = GameObject.Instantiate(s_prefab).GetComponent<Cartridge>();
			b.gameObject.SetActive(false);
			return b;
		}
	}
}
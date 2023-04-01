using UnityEngine;

namespace VsR {
	public static class BulletDecalPoolManager {
		private const string PREFAB_PATH = "Prefabs/Pooled/BulletDecal";
		public const int POOL_SIZE = 100;
		private static Decal s_prefab;
		private static Decal[] s_pool;
		private static int s_nextIdx = 0;

		[RuntimeInitializeOnLoadMethod]
		private static void Init() {
			s_prefab = Resources.Load<Decal>(PREFAB_PATH);
			s_pool = new Decal[POOL_SIZE];
			for (int i = 0; i < POOL_SIZE; ++i) {
				Decal decal = s_pool[i] = GameObject.Instantiate<Decal>(s_prefab);
				GameObject.DontDestroyOnLoad(decal);
				decal.gameObject.SetActive(false);
			}
		}

		public static void Spawn(Vector3 position, Vector3 normal) {
			Decal decal = s_pool[s_nextIdx];
			decal.gameObject.SetActive(true);

			decal.transform.position = position;
			decal.transform.LookAt(position + normal, Vector3.up);

			Vector3 rotation = decal.transform.localEulerAngles;
			rotation.z = Random.Range(0, 360);
			decal.transform.localEulerAngles = rotation;

			s_nextIdx = (s_nextIdx + 1) % POOL_SIZE;
		}
	}
}
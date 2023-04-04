using UnityEngine;
using UnityEngine.Pool;

namespace VsR {
	public static class MuzzleFlashPoolManager {
		private const int DEFAULT_SIZE = 4;
		private const int MAX_SIZE = 10;
		private const string PREFAB_PATH = "Prefabs/Pooled/MuzzleFlash";
		private static GameObject s_prefab;
		public static ObjectPool<MuzzleFlash> Pool { get; private set; }

		[RuntimeInitializeOnLoadMethod()]
		private static void Init() {
			s_prefab = (GameObject)Resources.Load(PREFAB_PATH);
			Pool = new ObjectPool<MuzzleFlash>(CreatePooledMuzzleFlash, null, null, (MuzzleFlash p) => GameObject.Destroy(p.gameObject), true, DEFAULT_SIZE, MAX_SIZE);
		}

		public static void Spawn(Transform barrelEndPoint, WeaponData data) {
			MuzzleFlash muzzleFlash = Pool.Get();
			muzzleFlash.gameObject.SetActive(true);

			muzzleFlash.transform.position = barrelEndPoint.position;
			muzzleFlash.transform.forward = barrelEndPoint.forward;
			muzzleFlash.transform.localScale = Vector3.one * data.muzzleFlashData.scale;

			muzzleFlash.m_particleSystem.Play();
		}

		private static MuzzleFlash CreatePooledMuzzleFlash() {
			MuzzleFlash muzzleFlash = GameObject.Instantiate(s_prefab).GetComponent<MuzzleFlash>();
			muzzleFlash.gameObject.SetActive(false);
			return muzzleFlash;
		}
	}
}
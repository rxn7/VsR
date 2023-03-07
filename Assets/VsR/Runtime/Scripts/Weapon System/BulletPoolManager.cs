using UnityEngine;
using UnityEngine.Pool;

namespace VsR {
	public class BulletPoolManager : SingletonBehaviour<BulletPoolManager> {
		[RuntimeInitializeOnLoadMethod] private static void _CreateInstance() => CreateInstance();

		private GameObject m_prefab;
		private ObjectPool<Bullet> m_pool;

		protected override void Awake() {
			base.Awake();
			m_prefab = (GameObject)Resources.Load("Prefabs/Bullet");
			m_pool = new ObjectPool<Bullet>(CreatePooledBullet, (Bullet b) => b.Enable(), (Bullet b) => b.Disable(), (Bullet b) => Destroy(b), true, 30, 60);
		}

		private Bullet CreatePooledBullet() {
			Bullet b = Instantiate(m_prefab, transform, false).GetComponent<Bullet>();
			b.gameObject.SetActive(false);
			return b;
		}

		public void ReleaseBullet(Bullet b) => m_pool.Release(b);
		public Bullet GetPooledBullet() => m_pool.Get();
	}
}
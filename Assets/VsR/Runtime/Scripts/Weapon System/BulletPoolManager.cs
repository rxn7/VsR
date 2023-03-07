using UnityEngine;
using UnityEngine.Pool;

namespace VsR {
	public class BulletPoolManager : SingletonBehaviour<BulletPoolManager> {
		[RuntimeInitializeOnLoadMethod] private static void _CreateInstance() => CreateInstance();

		private GameObject m_prefab;
		public ObjectPool<Bullet> Pool { get; private set; }

		protected override void Awake() {
			base.Awake();
			m_prefab = (GameObject)Resources.Load("Prefabs/Bullet");
			Pool = new ObjectPool<Bullet>(CreatePooledBullet, (Bullet b) => b.Enable(), (Bullet b) => b.Disable(), (Bullet b) => Destroy(b), true, 30, 60);
		}

		private Bullet CreatePooledBullet() {
			Bullet b = Instantiate(m_prefab, transform, false).GetComponent<Bullet>();
			b.gameObject.SetActive(false);
			return b;
		}
	}
}
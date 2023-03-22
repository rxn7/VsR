using UnityEngine;

namespace VsR {
	public class ShootingTarget : MonoBehaviour, IHIttable {
		[SerializeField] private AudioClip m_hitSound;
		[SerializeField] private Math.FloatRange m_scoreRange = new Math.FloatRange(5, 10);
		private MeshFilter m_meshFilter;

		private float m_size;

		private void Awake() {
			m_meshFilter = GetComponent<MeshFilter>();
			m_size = m_meshFilter.mesh.bounds.extents.x * transform.lossyScale.x;
		}

		public void OnHit(Vector3 point) {
			SoundPoolManager.Instance.PlaySound(m_hitSound, transform.position, Random.Range(0.95f, 1.05f), 10, 0.5f);

			Vector3 distanceVector = transform.InverseTransformDirection(point - transform.position);
			distanceVector.z = 0.0f;

			float distance = distanceVector.magnitude;
			int score = Mathf.CeilToInt(Mathf.Lerp(m_scoreRange.min, m_scoreRange.max, (m_size - distance) / m_size));

			Debug.Log($"d: {distance}, s: {score}");
		}
	}
}
using System;
using UnityEngine;
using VsR.Math;

namespace VsR {
    [ExecuteAlways]
	[RequireComponent(typeof(Rigidbody))]
	public class Cartridge : MonoBehaviour {
		public const float EJECT_LIFE_TIME_SECS = 3.0f;
		private static GameObject s_prefab = null;

        public static GameObject PREFAB {
            get {
                if(s_prefab == null) {
                    s_prefab = (GameObject)Resources.Load("Prefabs/Cartridge");
                }

                return s_prefab;
            }
        }

		[SerializeField] private MeshRenderer m_bulletMeshRenderer;
		[SerializeField] private MeshFilter m_bulletMeshFilter;
		[SerializeField] private MeshFilter m_cartridgeMeshFilter;

		private Rigidbody m_rb;
		private bool m_ejected = false;
		private CartridgeData m_data;

        public CartridgeData data => m_data;

        public bool hasBullet {
            get => m_bulletMeshRenderer.enabled;
            set {
                m_bulletMeshRenderer.enabled = value;
            }
        }

		private void Awake() {
			m_rb = GetComponent<Rigidbody>();
            m_rb.isKinematic = true;
            m_rb.detectCollisions = false;
		}

        public void SetData(CartridgeData data, bool _hasBullet) {
            m_data = data;
            hasBullet = _hasBullet;

			m_cartridgeMeshFilter.mesh = data.cartridgeMesh;
			m_bulletMeshFilter.mesh = data.bulletMesh;
        }

		public void Eject(Vector3 velocity, Transform trans, float force, float torque, FloatRange randomness) {
            if(m_data == null)
                throw new NullReferenceException("m_data of the cartridge is not set");

            m_rb.isKinematic = false;
            m_rb.detectCollisions = true;

			Vector3 random = new Vector3(randomness.RandomValue(), randomness.RandomValue(), randomness.RandomValue());
			Vector3 randomAngular = new Vector3(randomness.RandomValue(), randomness.RandomValue(), randomness.RandomValue());

            transform.SetParent(null, true);

            m_rb.isKinematic = false;
			m_rb.velocity = Vector3.Scale(transform.up, random) * force + velocity;
			m_rb.angularVelocity = Vector3.Scale(-transform.right, randomAngular) * torque;

			Destroy(gameObject, EJECT_LIFE_TIME_SECS);
		}

		private void OnCollisionEnter(Collision collision) {
			if (!m_ejected || m_data == null)
				return;

			float velocity = collision.relativeVelocity.magnitude * 0.2f;

			float pitch = Mathf.Clamp(velocity, 0.5f, 1.5f);
			// float volume = Mathf.Clamp01(velocity);

			SoundPoolManager.Instance.PlaySound(m_data.GetRandomCollideSound(), transform.position, pitch, 1.0f);
		}
	}
}
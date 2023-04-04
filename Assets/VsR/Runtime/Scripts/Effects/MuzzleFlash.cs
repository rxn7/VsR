using UnityEngine;

namespace VsR {
	[RequireComponent(typeof(ParticleSystem))]
	public class MuzzleFlash : MonoBehaviour {
		[HideInInspector] public ParticleSystem m_particleSystem;

		private void Awake() {
			m_particleSystem = GetComponent<ParticleSystem>();
		}

		private void OnParticleSystemStopped() {
			gameObject.SetActive(false);
			MuzzleFlashPoolManager.Pool.Release(this);
		}
	}
}

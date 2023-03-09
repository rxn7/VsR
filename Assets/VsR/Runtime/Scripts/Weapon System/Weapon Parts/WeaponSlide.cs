using UnityEngine;
using System.Collections;

namespace VsR {
	public class WeaponSlide : WeaponMovingPart {
		[SerializeField] protected float m_releaseAnimationSpeed = 0.03f;
		protected bool m_racked = false;

		public bool Racked => m_racked;

		protected override void Awake() {
			base.Awake();
		}

		protected override float UpdateSlideMovement() {
			float slidePercentage = base.UpdateSlideMovement();

			if (m_racked && slidePercentage < 0.6f)
				RackBack();
			else if (!m_racked && slidePercentage >= 0.99f)
				Rack();

			return slidePercentage;
		}

		protected void Rack() {
			SoundPoolManager.Instance.PlaySound(Weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));

			Weapon.TryToCock();

			m_racked = true;
		}

		protected void RackBack() {
			SoundPoolManager.Instance.PlaySound(Weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_racked = false;
		}

		protected IEnumerator ReleaseAnimation() {
			while (Vector3.Distance(transform.localPosition, m_initPosition) != 0) {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_initPosition, m_releaseAnimationSpeed * Time.deltaTime);
				yield return null;
			}
		}

		public override void Release() {
			base.Release();

			if (m_racked)
				RackBack();

			StartCoroutine(ReleaseAnimation());
		}
	}
}
using UnityEngine;
using System.Collections;

namespace VsR {
	public class WeaponSlide : WeaponMovingPart {
		public event System.Action onRacked;
		public event System.Action onRackedBack;

		[SerializeField] protected float m_releaseAnimationSpeed = 0.01f;
		protected bool m_racked = false;
		private float m_slidePercentage;

		public bool Racked => m_racked;
		public bool IsMoving => transform.localPosition != m_initPosition;
		public float SlidePercentage => m_slidePercentage;

		protected override float UpdateSlideMovement() {
			m_slidePercentage = base.UpdateSlideMovement();

			if (m_racked && m_slidePercentage < 0.6f)
				RackBack();
			else if (!m_racked && m_slidePercentage >= 0.99f)
				Rack();

			return m_slidePercentage;
		}

		protected void Rack() {
			onRacked?.Invoke();
			SoundPoolManager.Instance.PlaySound(Weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));
			Weapon.TryToCock();
			m_racked = true;
		}

		protected void RackBack() {
			onRackedBack?.Invoke();
			SoundPoolManager.Instance.PlaySound(Weapon.Data.rackBackSound, transform.position, Random.Range(0.9f, 1.1f));
			m_racked = false;
		}

		protected IEnumerator ReleaseAnimation() {
			float distance;
			while ((distance = Vector3.Distance(transform.localPosition, m_initPosition)) != 0) {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_initPosition, m_releaseAnimationSpeed * Time.deltaTime);
				m_slidePercentage = distance / m_maxSlideValue;
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
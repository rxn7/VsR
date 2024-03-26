using UnityEngine;
using System.Collections;
using TriInspector;

namespace VsR {
	public class WeaponSlide : WeaponMovingPart {
        [Required] [field: SerializeField] public WeaponBolt Bolt { get; private set; }
		[SerializeField] protected float m_releaseAnimationSpeed = 0.03f;
		[SerializeField] protected float m_rackSlidePercentageThreshold = 0.3f;
		[SerializeField] protected float m_pullSlidePercentageThreshold = 0.6f;
        
		public bool IsPulledBack { get; private set; } = false;

		protected override float UpdateSlideMovement() {
			float slidePercentage = base.UpdateSlideMovement();

			if (IsPulledBack && slidePercentage < m_rackSlidePercentageThreshold)
				Rack();
			else if (!IsPulledBack && slidePercentage > m_pullSlidePercentageThreshold)
				Pull();

			return slidePercentage;
		}

		protected void Pull() {
			SoundPoolManager.Instance.PlaySound(Weapon.Data.pullSound, transform.position, Random.Range(0.9f, 1.1f));

            Weapon.EjectChamberedCartridge(true);

			IsPulledBack = true;
		}

		protected void Rack() {
            Weapon.Rack();
			SoundPoolManager.Instance.PlaySound(Weapon.Data.rackSound, transform.position, Random.Range(0.9f, 1.1f));
			IsPulledBack = false;
		}

		protected IEnumerator ReleaseAnimation() {
			while (Vector3.Distance(transform.localPosition, m_initPosition) != 0) {
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, m_initPosition, m_releaseAnimationSpeed * Time.deltaTime);
				yield return null;
			}
		}

		public override void Release() {
			base.Release();

			if (IsPulledBack)
				Rack();

			StartCoroutine(ReleaseAnimation());
		}
	}
}
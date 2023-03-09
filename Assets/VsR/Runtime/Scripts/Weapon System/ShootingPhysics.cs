using UnityEngine;

namespace VsR {
	public class ShootingPhysics : SingletonBehaviour<ShootingPhysics> {
		private int m_layerMask;

		protected override void Awake() {
			base.Awake();
			m_layerMask = LayerMask.GetMask("Default", "Ground", "Grab");
		}

		public void OnHit(Collider collider) {
			if (collider.gameObject.TryGetComponent<IHIttable>(out IHIttable hittable))
				hittable.OnHit();
		}

		public void LaserRaycast(Transform barrelEnd, WeaponData data) {
			if (data.shootingPhysicsType != WeaponData.ShootingPhysicsType.RaycastLaser) {
				Debug.LogWarning("Tried to fire RaycastLaser from a weapon that does not have RaycastLaser ShootingPhysicsType");
				return;
			}

			Ray ray = new Ray(barrelEnd.position, barrelEnd.forward);
			if (Physics.Raycast(ray, out RaycastHit hit, 500, m_layerMask))
				OnHit(hit.collider);
		}
	}
}

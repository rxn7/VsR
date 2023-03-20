using UnityEngine;

namespace VsR {
	public static class ShootingPhysics {
		private static int s_layerMask;

		[RuntimeInitializeOnLoadMethod]
		private static void Init() {
			s_layerMask = LayerMask.GetMask("Default", "Ground", "Grab");
		}

		private static void OnHit(Collider collider) {
			if (collider.gameObject.TryGetComponent<IHIttable>(out IHIttable hittable))
				hittable.OnHit();
		}

		public static void LaserRaycast(Transform barrelEnd, WeaponData data) {
			if (data.shootingPhysicsType != WeaponData.ShootingPhysicsType.RaycastLaser) {
				Debug.LogWarning("Tried to fire RaycastLaser from a weapon that does not have RaycastLaser ShootingPhysicsType");
				return;
			}

			Ray ray = new Ray(barrelEnd.position, barrelEnd.forward);
			if (Physics.Raycast(ray, out RaycastHit hit, 500, s_layerMask))
				OnHit(hit.collider);
		}
	}
}

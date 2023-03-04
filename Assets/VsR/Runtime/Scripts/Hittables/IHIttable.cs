using UnityEngine;

namespace VsR {
	public interface IHIttable {
		public abstract void OnHit(Collision collision);
	}
}
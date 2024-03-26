using TriInspector;
using UnityEngine;

namespace VsR {
    public class VelocityTracker : MonoBehaviour {
        [HideInEditMode] [ReadOnly] public Vector3 velocity;
        private Vector3 m_previousPosition;

        public void Update() {
			velocity = (transform.position - m_previousPosition) / Time.unscaledDeltaTime;
            m_previousPosition = transform.position;
        }
    }
}

using TriInspector;
using UnityEngine;

namespace VsR {
    [ExecuteAlways]
    public class FollowTransform : MonoBehaviour {
        [Required] [SerializeField] private Transform m_target; 
        [SerializeField] private Vector3 m_offset;
        [SerializeField] private Vector3 m_rotationOffset;

        private void LateUpdate() {
            transform.position = m_target.position + m_target.TransformDirection(m_offset);
            transform.eulerAngles = m_target.eulerAngles + m_rotationOffset;
        }
    }
}

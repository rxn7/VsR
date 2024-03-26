using UnityEngine;

namespace VsR.Runtime.Helpers {
    public static class MonoBehaviourHelper {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            if(gameObject.TryGetComponent(out T component)) {
                return component;
            }

            return gameObject.AddComponent<T>();
        }
    }
}
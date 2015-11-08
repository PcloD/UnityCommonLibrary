using UnityEngine;

namespace UnityCommonLibrary {
    public static class GOUtility {

        public static T AddOrGetComponent<T>(this GameObject obj) where T : Component {
            T component = obj.GetComponent<T>();
            if(component == null) {
                return obj.AddComponent<T>();
            }
            else {
                return component;
            }
        }

        public static T EnsureComponent<T>(this GameObject obj) where T : Component {
            if(obj == null) {
                throw new System.ArgumentNullException("obj");
            }
            T component = obj.GetComponent<T>();
            if(component == null) {
                throw new System.NullReferenceException(string.Format("{0} does not exist on {1}", typeof(T).Name, obj));
            }
            else {
                return obj.AddComponent<T>();
            }
        }

        public static void SetLayerRecursive(this GameObject obj, int layer) {
            obj.layer = layer;
            foreach(Transform child in obj.transform) {
                child.gameObject.SetLayerRecursive(layer);
            }
        }

        public static string GetPath(GameObject obj) {
            var path = "/" + obj.name;
            while(obj.transform.parent != null) {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        public static void Toggle(bool enabled, params GameObject[] gameObjects) {
            foreach(var go in gameObjects) {
                if(go != null) {
                    go.SetActive(enabled);
                }
            }
        }

    }
}
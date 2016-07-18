using UnityEngine;
using UnityEngine.Assertions;

namespace UnityCommonLibrary.Utilities
{
    public static class GameObjectUtility
    {

        public static T AddOrGetComponent<T>(this GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                return obj.AddComponent<T>();
            }
            else
            {
                return component;
            }
        }

        public static T AssertComponent<T>(this GameObject obj) where T : Component
        {
            Assert.IsTrue(obj);
            Assert.IsNotNull(obj);
            var component = obj.GetComponent<T>();
            Assert.IsTrue(component);
            Assert.IsNotNull(component);
            return obj.AddComponent<T>();
        }

        public static void SetLayerRecursive(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                child.gameObject.SetLayerRecursive(layer);
            }
        }

        public static string GetPath(GameObject obj)
        {
            var sb = new System.Text.StringBuilder("/" + obj.name);
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                sb.Insert(0, "/" + obj.name);
            }
            return sb.ToString();
        }

        public static void Toggle(bool enabled, params GameObject[] gameObjects)
        {
            foreach (var go in gameObjects)
            {
                if (go != null)
                {
                    go.SetActive(enabled);
                }
            }
        }

    }
}
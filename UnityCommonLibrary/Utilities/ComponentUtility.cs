using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityCommonLibrary.Utility
{
    public static class ComponentUtility
    {
        public static T Create<T>() where T : Component
        {
            return Create<T>(typeof(T).Name);
        }

        public static T Create<T>(string name) where T : Component
        {
            return new GameObject(name).AddComponent<T>();
        }

        public static Component Create(Type t)
        {
            return Create(t, t.Name);
        }

        public static Component Create(Type t, string name)
        {
            return new GameObject(name).AddComponent(t);
        }

        public static void SetEnabledAll(bool enabled, params Behaviour[] behaviors)
        {
            foreach (var b in behaviors)
            {
                if (b != null)
                {
                    b.enabled = enabled;
                }
            }
        }

        public static bool TryDestroy<T>(Component c) where T : Component
        {
            var t = c.GetComponent<T>();
            if (t)
            {
                Object.Destroy(t);
                return true;
            }
            return false;
        }

        public static bool TryDestroyAll<T>(Component c) where T : Component
        {
            var all = c.GetComponents<T>();
            for (var i = 0; i < all.Length; i++)
            {
                Object.Destroy(all[i]);
            }
            return all.Length > 0;
        }
    }
}
using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static bool isShuttingDown;
        private static T instance;

        public static T Instance
        {
            get
            {
                if (isShuttingDown)
                {
                    return null;
                }
                if (instance == null)
                {
                    FindOrCreate();
                }
                return instance;
            }
        }

        public static void EnsureExists()
        {
            var t = Instance;
        }
        private static void FindOrCreate()
        {
            var all = FindObjectsOfType<T>();
            instance = all.Length == 0 ? ComponentUtility.Create<T>() : all[0];
            if (all.Length > 1)
            {
                Debug.LogError(string.Format("FindObjectsOfType<{0}>().Length == {1}", typeof(T).Name, all.Length));
            }
            DontDestroyOnLoad(instance);
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            if (instance == null)
            {
                instance = (T)this;
            }
        }
        protected virtual void OnApplicationQuit()
        {
            isShuttingDown = true;
        }
    }

    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }

    public abstract class ActivatorSingleton<T> where T : ActivatorSingleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = System.Activator.CreateInstance<T>();
                }
                return instance;
            }
        }
    }
}
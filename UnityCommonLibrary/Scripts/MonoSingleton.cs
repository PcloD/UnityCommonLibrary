using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static bool isShuttingDown;
        private static T _get;

        public static T get
        {
            get
            {
                if (isShuttingDown)
                {
                    return null;
                }
                if (_get == null)
                {
                    FindOrCreate();
                }
                return _get;
            }
        }

        public static void EnsureExists()
        {
            var t = get;
        }
        private static void FindOrCreate()
        {
            var all = FindObjectsOfType<T>();
            _get = all.Length == 0 ? ComponentUtility.Create<T>() : all[0];
            if (all.Length > 1)
            {
                Debug.LogError(string.Format("FindObjectsOfType<{0}>().Length == {1}", typeof(T).Name, all.Length));
            }
            DontDestroyOnLoad(_get);
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            if (_get == null)
            {
                _get = (T)this;
            }
        }
        protected virtual void OnApplicationQuit()
        {
            isShuttingDown = true;
        }
    }

    public abstract class Singleton<T> where T : Singleton<T>, new()
    {

        private static T _get;
        public static T get
        {
            get
            {
                if (_get == null)
                {
                    _get = new T();
                }
                return _get;
            }
        }
    }

    public abstract class ActivatorSingleton<T> where T : ActivatorSingleton<T>
    {

        private static T _get;
        public static T get
        {
            get
            {
                if (_get == null)
                {
                    _get = System.Activator.CreateInstance<T>();
                }
                return _get;
            }
        }
    }
}
using System;
using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        private static bool _isShuttingDown;

        public static T Instance
        {
            get
            {
                if (_isShuttingDown)
                {
                    return null;
                }
                if (_instance == null)
                {
                    FindOrCreate();
                }
                return _instance;
            }
        }

        public static void EnsureExists()
        {
            var t = Instance;
        }

        private static void FindOrCreate()
        {
            var all = FindObjectsOfType<T>();
            _instance = all.Length == 0 ? ComponentUtility.Create<T>() : all[0];
            if (all.Length > 1)
            {
                UCLCore.Logger.LogFormat(LogType.Error, "FindObjectsOfType<{0}>().Length == {1}",
                    typeof(T).Name, all.Length);
            }
            DontDestroyOnLoad(_instance);
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance == null)
            {
                _instance = (T) this;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _isShuttingDown = true;
        }
    }

    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }

    public abstract class ActivatorSingleton<T> where T : ActivatorSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance<T>();
                }
                return _instance;
            }
        }
    }
}
using UnityEngine;

namespace UnityCommonLibrary
{
    public abstract class ScriptableSingleton<T> : ScriptableObject
        where T : ScriptableSingleton<T>
    {
        private static T _get;
        private static bool _isShuttingDown;

        public static T Get
        {
            get
            {
                if (_isShuttingDown)
                {
                    return null;
                }
                if (!_get)
                {
                    _get = CreateInstance<T>();
                }
                return _get;
            }
        }

        public static void EnsureExists()
        {
            if (!_get)
            {
                _get = CreateInstance<T>();
            }
        }
    }
}
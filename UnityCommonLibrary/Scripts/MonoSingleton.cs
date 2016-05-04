using UnityCommonLibrary.Utilities;
using UnityEngine;

namespace UnityCommonLibrary
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
        private static T _get;

        public static T get {
            get {
                if(_get == null) {
                    EnsureExists();
                }
                return _get;
            }
        }

        private static void EnsureExists() {
            var all = FindObjectsOfType<T>();
            if(all.Length == 0) {
                _get = ComponentUtility.Create<T>();
            }
            else {
                _get = all[0];
            }

            if(all.Length > 1) {
                Debug.LogError(string.Format("FindObjectsOfType<{0}>().Length == {1}", typeof(T).Name, all.Length));
            }
        }

        public static T DummyCreate() {
            return get;
        }
    }

    public abstract class Singleton<T> where T : Singleton<T>, new() {
        private static T _get;

        public static T get {
            get {
                if(_get == null) {
                    _get = new T();
                }
                return _get;
            }
        }
    }
}
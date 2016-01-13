using UnityEngine;

namespace UnityCommonLibrary {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        private static bool appQuitting;
        private static object @lock = new object();
        private static T _get;

        public static T get {
            get {
                return GetSingleton();
            }
        }

        public static T DummyCreate() {
            return get;
        }

        private static readonly string TYPE_STRING = string.Format("Singleton<{0}>", typeof(T).Name);
        private static T GetSingleton() {
            lock (@lock) {
                if(appQuitting) {
                    Debug.Log(TYPE_STRING + " REQUESTED: APP IS QUITTING");
                    return null;
                }

                if(_get == null) {
                    var allInstances = FindObjectsOfType<T>();
                    if(allInstances.Length > 1) {
                        Debug.LogError(TYPE_STRING + " duplicates found.");
                        _get = allInstances[0];
                    }
                    else if(allInstances.Length == 0) {
                        _get = new GameObject(TYPE_STRING).AddComponent<T>();
                    }
                    else {
                        _get = allInstances[0];
                    }
                }
                return _get;
            }
        }

        protected virtual void Awake() {
            DontDestroyOnLoad(this);
        }

        private void OnApplicationQuit() {
            appQuitting = true;
        }
    }
}
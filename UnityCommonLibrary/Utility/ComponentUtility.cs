using UnityEngine;

namespace UnityCommonLibrary {
    public static class ComponentUtility {

        public static T Create<T>() where T : Component {
            return Create<T>(typeof(T).Name);
        }

        public static T Create<T>(string objName) where T : Component {
            return new GameObject(objName).AddComponent<T>();
        }

        public static void Toggle(bool enabled, params Behaviour[] behaviors) {
            foreach(var b in behaviors) {
                if(b != null) {
                    b.enabled = enabled;
                }
            }
        }

    }
}
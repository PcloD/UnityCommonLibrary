using UnityEngine;

namespace UnityCommonLibrary {
    public static class BehaviorUtils {

        public static void Toggle(bool enabled, params Behaviour[] behaviors) {
            foreach(var b in behaviors) {
                b.enabled = enabled;
            }
        }

    }
}

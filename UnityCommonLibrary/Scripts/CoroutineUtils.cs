using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary {

    public static class CoroutineUtils {
        public static IEnumerator WaitForSecondsUnscaled(float seconds) {
            var start = Time.realtimeSinceStartup;
            while(Time.realtimeSinceStartup - start < seconds) {
                yield return null;
            }
        }
    }

}
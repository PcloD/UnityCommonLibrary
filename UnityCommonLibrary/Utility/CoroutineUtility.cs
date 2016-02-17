using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary.Utilities {

    public static class CoroutineUtility {
        static MonoBehaviour dummyObj;

        public static IEnumerator WaitForSecondsUnscaled(float seconds) {
            var start = Time.realtimeSinceStartup;
            while(Time.realtimeSinceStartup - start < seconds) {
                yield return null;
            }
        }

        public static Coroutine StartCoroutine(IEnumerator routine) {
            if(dummyObj == null) {
                var obj = new GameObject("CoroutineUtilityDummyObj");
                dummyObj = obj.AddComponent<MonoBehaviour>();
                Object.DontDestroyOnLoad(dummyObj);
            }
            return dummyObj.StartCoroutine(routine);
        }

        public static void StopAllCoroutines() {
            dummyObj.StopAllCoroutines();
        }

        public static void StopCoroutine(IEnumerator routine) {
            dummyObj.StopCoroutine(routine);
        }

        public static void StopCoroutine(Coroutine routine) {
            dummyObj.StopCoroutine(routine);
        }

    }

}
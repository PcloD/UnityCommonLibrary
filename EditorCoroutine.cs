#if UNITY_EDITOR

using UnityEditor;

#else
using UnityEngine;
#endif

using System.Collections;

namespace UnityCommonLibrary {
    public class EditorCoroutine {
        IEnumerator routine;

        EditorCoroutine(IEnumerator routine) {
            this.routine = routine;
        }

        public static EditorCoroutine Start(IEnumerator _routine) {
            var coroutine = new EditorCoroutine(_routine);
            coroutine.Start();
            return coroutine;
        }

        void Start() {
#if UNITY_EDITOR
            EditorApplication.update += EditorUpdate;
#else
            Debug.LogError("NOT IN EDITOR");
#endif
        }

        public void Stop() {
#if UNITY_EDITOR
            EditorApplication.update -= EditorUpdate;
#else
            Debug.LogError("NOT IN EDITOR");
#endif
        }

        void EditorUpdate() {
            /* NOTE: no need to try/catch MoveNext,
             * if an IEnumerator throws its next iteration returns false.
             * Also, Unity probably catches when calling EditorApplication.update.
             */
            if(!routine.MoveNext()) {
                Stop();
            }
        }
    }
}
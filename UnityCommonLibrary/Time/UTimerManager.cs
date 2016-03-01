using UnityEngine;

namespace UnityCommonLibrary.Time {
    public sealed class UTimerManager : MonoSingleton<UTimerManager> {
        [RuntimeInitializeOnLoadMethod]
        private static void AutoCreate() {
            DummyCreate();
        }

        private void Update() {
            foreach(var t in UTimer.allReadonly) {
                t.Tick();
            }
        }
    }
}

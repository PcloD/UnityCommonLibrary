using System.Collections.Generic;

namespace UnityCommonLibrary.Time {
    public sealed class UTimerManager : MonoSingleton<UTimerManager> {
        private HashSet<UTimer> timers = new HashSet<UTimer>();
        public List<UTimer> timersPublic = new List<UTimer>();

        internal void Register(UTimer timer) {
            if(!timers.Contains(timer)) {
                timers.Add(timer);
                if(UnityEngine.Application.isEditor) {
                    timersPublic.Add(timer);
                }
            }
        }

        internal void Deregister(UTimer timer) {
            timers.Remove(timer);
            if(UnityEngine.Application.isEditor) {
                timersPublic.Remove(timer);
            }
        }

        private void Update() {
            timers.RemoveWhere(t => t == null);
            if(UnityEngine.Application.isEditor) {
                timersPublic.RemoveAll(t => t == null);
            }
            foreach(var t in timers) {
                t.Tick();
            }
        }
    }
}

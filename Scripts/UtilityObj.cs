using System.Collections.Generic;

namespace UnityCommonLibrary {
    public delegate void OnTick();

    public sealed class UtilityObj : UCSingleton<UtilityObj> {
        HashSet<OnTick> updates = new HashSet<OnTick>();
        HashSet<OnTick> fixedUpdates = new HashSet<OnTick>();
        HashSet<OnTick> lateUpdates = new HashSet<OnTick>();

        List<OnTick> markedUpdates = new List<OnTick>();
        List<OnTick> markedFixedUpdates = new List<OnTick>();
        List<OnTick> markedLateUpdates = new List<OnTick>();

        void Update() {
            foreach(var e in updates) {
                e();
            }
            foreach(var e in markedUpdates) {
                updates.Remove(e);
            }
        }

        void FixedUpdate() {
            foreach(var e in fixedUpdates) {
                e();
            }
            foreach(var e in markedFixedUpdates) {
                fixedUpdates.Remove(e);
            }
        }

        void LateUpdate() {
            foreach(var e in lateUpdates) {
                e();
            }
            foreach(var e in markedLateUpdates) {
                lateUpdates.Remove(e);
            }
        }

        void OnDestroy() {
            Destroy(gameObject);
        }

        public static void RegisterUpdate(OnTick onTick) {
            get.updates.Add(onTick);
        }

        public static void RegisterFixedUpdate(OnTick onTick) {
            get.fixedUpdates.Add(onTick);
        }

        public static void RegisterLateUpdate(OnTick onTick) {
            get.lateUpdates.Add(onTick);
        }

        public static void UnregisterUpdate(OnTick onTick) {
            if(get.updates.Contains(onTick)) {
                get.markedUpdates.Add(onTick);
            }
        }

        public static void UnregisterFixedUpdate(OnTick onTick) {
            if(get.fixedUpdates.Contains(onTick)) {
                get.markedFixedUpdates.Add(onTick);
            }
        }

        public static void UnregisterLateUpdate(OnTick onTick) {
            if(get.lateUpdates.Contains(onTick)) {
                get.markedLateUpdates.Add(onTick);
            }
        }

    }

}
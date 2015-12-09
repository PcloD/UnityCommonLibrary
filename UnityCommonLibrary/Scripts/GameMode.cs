using UnityEngine;

namespace UnityCommonLibrary {
    public abstract class GameMode {
        int lastTickFrame = -1;

        public virtual void Setup() { }

        /// <summary>
        /// Ticks at most once per frame.
        /// </summary>
        public void Tick() {
            if(Time.frameCount > lastTickFrame) {
                lastTickFrame = Time.frameCount;
                InternalTick();
            }
        }

        protected virtual void InternalTick() { }
    }
}
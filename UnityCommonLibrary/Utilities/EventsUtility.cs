using System;

namespace UnityCommonLibrary.Utilities {
    public delegate void GameEvent();
    public delegate void GameEvent<S>(S source);
    public delegate void GameEvent<S, T>(S source, T args) where T : EventArgs;

    public static class EventsUtility {
        public static void Invoke(GameEvent gEvent) {
            if(gEvent != null) {
                gEvent();
            }
        }

        public static void Invoke<S>(GameEvent<S> gEvent, S source) {
            if(gEvent != null) {
                gEvent(source);
            }
        }

        public static void Invoke<S, T>(GameEvent<S, T> gEvent, S source, T args) where T : EventArgs {
            if(gEvent != null) {
                gEvent(source, args);
            }
        }
    }
}

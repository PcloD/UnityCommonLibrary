using System;
using UnityCommonLibrary.Time;

namespace UnityCommonLibrary.Utilities
{
    public static class TimeUtility {

        public static float GetCurrentTime(TimeMode mode) {
            switch(mode) {
                case TimeMode.Time:
                    return UnityEngine.Time.time;
                case TimeMode.UnscaledTime:
                    return UnityEngine.Time.unscaledTime;
                case TimeMode.RealtimeSinceStartup:
                    return UnityEngine.Time.realtimeSinceStartup;
                case TimeMode.FixedTime:
                    return UnityEngine.Time.fixedTime;
                default:
                    throw new Exception("Invalid TimeMode");
            }
        }

    }
}

using System;
using UnityCommonLibrary.Time;

namespace UnityCommonLibrary.Utilities
{
    public static class TimeUtility
    {

        public static float GetCurrentTime(TimeMode mode)
        {
            switch (mode)
            {
                case TimeMode.Time:
                    return UnityEngine.Time.time;
                case TimeMode.UnscaledTime:
                    return UnityEngine.Time.unscaledTime;
                case TimeMode.RealtimeSinceStartup:
                    return UnityEngine.Time.realtimeSinceStartup;
                case TimeMode.FixedTime:
                    return UnityEngine.Time.fixedTime;
                case TimeMode.DeltaTime:
                    return UnityEngine.Time.deltaTime;
                case TimeMode.UnscaledDeltaTime:
                    return UnityEngine.Time.unscaledDeltaTime;
                case TimeMode.SmoothDeltaTime:
                    return UnityEngine.Time.smoothDeltaTime;
                case TimeMode.FixedDeltaTime:
                    return UnityEngine.Time.fixedDeltaTime;
                case TimeMode.TimeSinceLevelLoad:
                    return UnityEngine.Time.timeSinceLevelLoad;
                case TimeMode.One:
                    return 1f;
                default:
                    throw new Exception("Invalid TimeMode");
            }
        }

    }
}

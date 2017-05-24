using System;
using UnityCommonLibrary.Time;
using UTime = UnityEngine.Time;

namespace UnityCommonLibrary.Utility
{
    public static class TimeUtility
    {
        public static float GetCurrentTime(TimeMode mode)
        {
            switch (mode)
            {
                case TimeMode.Time:
                    return UTime.time;
                case TimeMode.UnscaledTime:
                    return UTime.unscaledTime;
                case TimeMode.RealtimeSinceStartup:
                    return UTime.realtimeSinceStartup;
                case TimeMode.FixedTime:
                    return UTime.fixedTime;
                case TimeMode.DeltaTime:
                    return UTime.deltaTime;
                case TimeMode.UnscaledDeltaTime:
                    return UTime.unscaledDeltaTime;
                case TimeMode.SmoothDeltaTime:
                    return UTime.smoothDeltaTime;
                case TimeMode.FixedDeltaTime:
                    return UTime.fixedDeltaTime;
                case TimeMode.TimeSinceLevelLoad:
                    return UTime.timeSinceLevelLoad;
                case TimeMode.One:
                    return 1f;
                default:
                    throw new Exception("Invalid TimeMode");
            }
        }
    }
}
namespace UnityCommonLibrary.Time
{
    public struct TimeSlice
    {
        public float time;
        public float unscaledTime;
        public float realtimeSinceStartup;

        public static TimeSlice Create()
        {
            return new TimeSlice()
            {
                time = UnityEngine.Time.time,
                unscaledTime = UnityEngine.Time.unscaledTime,
                realtimeSinceStartup = UnityEngine.Time.realtimeSinceStartup
            };
        }
    }
}
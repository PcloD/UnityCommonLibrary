namespace UnityCommonLibrary.Time
{
    public struct TimeSlice
    {
        public float time { get; private set; }
        public float unscaledTime { get; private set; }
        public float realtimeSinceStartup { get; private set; }
        public int frameCount { get; private set; }

        public static TimeSlice Create()
        {
            return new TimeSlice()
            {
                time = UnityEngine.Time.time,
                unscaledTime = UnityEngine.Time.unscaledTime,
                realtimeSinceStartup = UnityEngine.Time.realtimeSinceStartup,
                frameCount = UnityEngine.Time.frameCount
            };
        }
    }
}
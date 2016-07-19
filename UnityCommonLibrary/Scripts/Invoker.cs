using System;
using UnityCommonLibrary.Time;

namespace UnityCommonLibrary
{
    public static class Invoker
    {
        public static void Invoke(Action a, float time)
        {
            Invoke(a, TimeMode.Time, time);
        }
        public static void Invoke(Action a, TimeMode mode, float time)
        {
            if (time == 0f)
            {
                a();
            }
            else
            {
                var timer = new UTimer(mode);
                timer.duration = time;
                timer.TimerElapsed += () =>
                {
                    timer.Stop();
                    if (a != null)
                    {
                        a();
                    }
                    timer = null;
                };
                timer.Start();
            }
        }
    }
}

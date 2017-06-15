using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityCommonLibrary.Utility;

namespace UnityCommonLibrary.Time
{
    /// <summary>
    ///     A timer that either operates in a standard timer or a stopwatch mode.
    /// </summary>
    public sealed class UTimer
    {
        public enum Mechanism
        {
            /// <summary>
            ///     Counts down and elapses when <see cref="UTimer.Value" /> is less than or equal to 0.
            /// </summary>
            Timer,

            /// <summary>
            ///     Counts up and elapses in intervals when <see cref="UTimer.Value" />
            ///     is greater than or equal to <see cref="UTimer._nextInterval" /> * <see cref="UTimer.Duration" />
            /// </summary>
            Stopwatch
        }

        /// <summary>
        ///     Represents the current playback MachineStatus of the timer.
        /// </summary>
        public enum State
        {
            Stopped,
            Running,
            Paused
        }

        /// <summary>
        ///     All existing timers.
        /// </summary>
        private static readonly List<UTimer> _allTimers = new List<UTimer>();

        /// <summary>
        ///     A readonly wrapper for <see cref="_allTimers" />
        /// </summary>
        public static ReadOnlyCollection<UTimer> AllReadonly =
            new ReadOnlyCollection<UTimer>(_allTimers);


        /// <summary>
        ///     How long the timer takes to elapse in either direction.
        /// </summary>
        public float Duration;

        /// <summary>
        ///     The method in which to determine time.
        /// </summary>
        public TimeMode TimeMode;

        /// <summary>
        ///     The target interval multiplier for a timer in Stopwatch TimerMechanism.
        /// </summary>
        private uint _nextInterval = 1;

        public delegate void OnTimerElapsed();

        public event OnTimerElapsed TimerElapsed;

        /// <summary>
        ///     Has the timer elapsed based on the <see cref="TimerMechanism" />
        /// </summary>
        public bool HasElapsed { get; private set; }

        /// <summary>
        ///     The current interval, returns 0 if <see cref="TimerMechanism" />
        ///     == TimerState.
        /// </summary>
        public uint Interval
        {
            get { return TimerMechanism == Mechanism.Timer ? 0 : _nextInterval - 1; }
        }

        /// <summary>
        ///     The time in which the timer was last paused.
        /// </summary>
        public float LastPauseTime { get; private set; }

        /// <summary>
        ///     Returns a new TimeSpan object representing the current value.
        /// </summary>
        public TimeSpan Span
        {
            get { return TimeSpan.FromSeconds(Value); }
        }

        /// <summary>
        ///     The time in which the timer was started.
        /// </summary>
        public float StartTime { get; private set; }

        /// <summary>
        ///     The direction in which the timer will tick.
        /// </summary>
        public Mechanism TimerMechanism { get; }

        /// <summary>
        ///     The current playback TimerState of the timer.
        /// </summary>
        public State TimerState { get; private set; }

        /// <summary>
        ///     The total amount of time the timer has spent paused.
        /// </summary>
        public float TotalPauseTime { get; private set; }

        /// <summary>
        ///     The current TimerState or Stopwatch value.
        /// </summary>
        public float Value { get; private set; }

        /// <summary>
        ///     Creates and Resets a new timer in TimerState Mechanism in Time TimeMode.
        /// </summary>
        public UTimer() : this(Mechanism.Timer, TimeMode.Time) { }

        /// <summary>
        ///     Creates and Resets a new timer in Time TimeMode.
        /// </summary>
        /// <param name="timerMechanism">The directional TimerMechanism to use.</param>
        public UTimer(Mechanism timerMechanism) : this(timerMechanism, TimeMode.Time) { }

        /// <summary>
        ///     Creates and Resets a new timer in TimerState Mechanism.
        /// </summary>
        /// <param name="timeMode">The time counting mode to use.</param>
        public UTimer(TimeMode timeMode) : this(Mechanism.Timer, timeMode) { }

        /// <summary>
        ///     Creates and Resets a new timer.
        /// </summary>
        /// <param name="timerMechanism">The directional TimerMechanism to use.</param>
        /// <param name="timeMode">The time counting mode to use.</param>
        public UTimer(Mechanism timerMechanism, TimeMode timeMode)
        {
            _allTimers.Add(this);
            TimerMechanism = timerMechanism;
            TimeMode = timeMode;
            Reset();
        }

        ~UTimer()
        {
            _allTimers.Remove(this);
        }

        /// <summary>
        ///     This must be called by some external script for timers to update.
        /// </summary>
        public static void TickAll()
        {
            foreach (var t in _allTimers)
            {
                t.Tick();
            }
        }

        /// <summary>
        ///     If currently running, pauses the timer.
        /// </summary>
        public void Pause()
        {
            if (TimerState == State.Running)
            {
                TimerState = State.Paused;
                LastPauseTime = TimeUtility.GetCurrentTime(TimeMode);
            }
        }

        /// <summary>
        ///     Resets all readonly public and private timer
        ///     counters.
        /// </summary>
        public void Reset()
        {
            TimerState = State.Stopped;
            StartTime = 0f;
            TotalPauseTime = 0f;
            _nextInterval = 1;
            HasElapsed = false;
        }

        /// <summary>
        ///     Resets and starts the timer.
        /// </summary>
        public void Restart()
        {
            TimerState = State.Stopped;
            Start();
        }

        /// <summary>
        ///     If currently paused, resumes the timer.
        /// </summary>
        public void Resume()
        {
            if (TimerState == State.Paused)
            {
                TimerState = State.Running;
                TotalPauseTime += TimeUtility.GetCurrentTime(TimeMode) - LastPauseTime;
            }
        }

        /// <summary>
        ///     If currently stopped, resets and then starts the timer.
        /// </summary>
        public void Start()
        {
            if (TimerState == State.Stopped)
            {
                Reset();
                StartTime = TimeUtility.GetCurrentTime(TimeMode);
                TimerState = State.Running;
            }
        }

        /// <summary>
        ///     If not currently stopped, stops the timer.
        /// </summary>
        public void Stop()
        {
            if (TimerState != State.Stopped)
            {
                TimerState = State.Stopped;
            }
        }

        public override string ToString()
        {
            return Span.ToString();
        }

        private void FireElapsedEvent()
        {
            HasElapsed = true;
            if (TimerElapsed != null)
            {
                TimerElapsed();
            }
        }

        /// <summary>
        ///     Updates the timer's counters and values.
        /// </summary>
        private void Tick()
        {
            // Only update if running.
            if (TimerState != State.Running)
            {
                return;
            }

            switch (TimerMechanism)
            {
                case Mechanism.Timer:
                    Value = Duration -
                            (TimeUtility.GetCurrentTime(TimeMode) - StartTime) -
                            TotalPauseTime;
                    if (Value <= 0f)
                    {
                        FireElapsedEvent();
                    }
                    break;
                case Mechanism.Stopwatch:
                    Value = TimeUtility.GetCurrentTime(TimeMode) - StartTime -
                            TotalPauseTime;
                    // To determine when to elapse the stopwatch, the target is determined by
                    // way of using an interval counter.
                    if (Value >= Duration * _nextInterval)
                    {
                        _nextInterval++;
                        FireElapsedEvent();
                    }
                    break;
            }
        }
    }
}
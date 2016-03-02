using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityCommonLibrary.Utilities;

namespace UnityCommonLibrary.Time {
    public sealed class UTimer {
        private static List<UTimer> all = new List<UTimer>();
        public static ReadOnlyCollection<UTimer> allReadonly = new ReadOnlyCollection<UTimer>(all);

        public delegate void OnTimerElapsed();
        public event OnTimerElapsed TimerElapsed;

        public float duration;
        public TimeMode timeMode;

        public bool hasElapsed { get; private set; }
        public Mode mode { get; private set; }
        public State state { get; private set; }
        public float startTime { get; private set; }
        public float totalPauseTime { get; private set; }
        public float lastPauseTime { get; private set; }
        public float value { get; private set; }
        public TimeSpan span {
            get {
                return TimeSpan.FromSeconds(value);
            }
        }
        public uint interval {
            get {
                return mode == Mode.Timer ? 0 : nextInterval - 1;
            }
        }

        private uint nextInterval = 1;

        public UTimer() : this(Mode.Timer, TimeMode.Time) { }

        public UTimer(Mode mode) : this(mode, TimeMode.Time) { }

        public UTimer(TimeMode timeMode) : this(Mode.Timer, timeMode) { }

        public UTimer(Mode mode, TimeMode timeMode) {
            all.Add(this);
            this.mode = mode;
            this.timeMode = timeMode;
            Reset();
        }

        ~UTimer() {
            all.Remove(this);
        }

        #region Controls
        public void Start() {
            if(state == State.Stopped) {
                Reset();
                startTime = TimeUtility.GetCurrentTime(timeMode);
                state = State.Running;
            }
        }

        public void Pause() {
            if(state == State.Running) {
                state = State.Paused;
                lastPauseTime = TimeUtility.GetCurrentTime(timeMode);
            }
        }

        public void Resume() {
            if(state == State.Paused) {
                state = State.Running;
                totalPauseTime += TimeUtility.GetCurrentTime(timeMode) - lastPauseTime;
            }
        }

        public void Stop() {
            if(state != State.Stopped) {
                state = State.Stopped;
            }
        }

        public void Reset() {
            state = State.Stopped;
            startTime = 0f;
            totalPauseTime = 0f;
            nextInterval = 1;
            hasElapsed = false;
        }

        public void Restart() {
            state = State.Stopped;
            Start();
        }
        #endregion

        internal void Tick() {
            if(state != State.Running) {
                return;
            }

            switch(mode) {
                case Mode.Timer:
                    value = (duration - (TimeUtility.GetCurrentTime(timeMode) - startTime)) - totalPauseTime;
                    if(value <= 0f) {
                        FireElapsedEvent();
                    }
                    break;
                case Mode.Stopwatch:
                    value = (TimeUtility.GetCurrentTime(timeMode) - startTime) - totalPauseTime;
                    if(value >= duration * nextInterval) {
                        nextInterval++;
                        FireElapsedEvent();
                    }
                    break;
            }
        }

        private void FireElapsedEvent() {
            hasElapsed = true;
            if(TimerElapsed != null) {
                TimerElapsed();
            }
        }

        public override string ToString() {
            return span.ToString();
        }

        public enum State {
            Stopped,
            Running,
            Paused
        }

        public enum Mode {
            Timer,
            Stopwatch
        }
    }
}

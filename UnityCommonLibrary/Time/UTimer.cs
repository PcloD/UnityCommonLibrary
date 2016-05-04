using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityCommonLibrary.Utilities;

namespace UnityCommonLibrary.Time
{
    /// <summary>
    /// A timer that either operates in a standard timer or a stopwatch mode.
    /// </summary>
    public sealed class UTimer {
		/// <summary>
		/// All existing timers.
		/// </summary>
		private static readonly List<UTimer> all = new List<UTimer>();
		/// <summary>
		/// A readonly wrapper for <see cref="all"/>
		/// </summary>
		public static ReadOnlyCollection<UTimer> allReadonly = new ReadOnlyCollection<UTimer>(all);

		public delegate void OnTimerElapsed();
		/// <summary>
		/// Fires when the timer elapses based on the <see cref="mechanism"/>
		/// </summary>
		public event OnTimerElapsed TimerElapsed;

		/// <summary>
		/// How long the timer takes to elapse in either direction.
		/// </summary>
		public float duration;
		/// <summary>
		/// The method in which to determine time.
		/// </summary>
		public TimeMode timeMode;

		#region Properties
		/// <summary>
		/// Has the timer elapsed based on the <see cref="mechanism"/>
		/// </summary>
		public bool hasElapsed { get; private set; }
		/// <summary>
		/// The direction in which the timer will tick.
		/// </summary>
		public Mechanism mechanism { get; private set; }
		/// <summary>
		/// The current playback state of the timer.
		/// </summary>
		public State state { get; private set; }
		/// <summary>
		/// The time in which the timer was started.
		/// </summary>
		public float startTime { get; private set; }
		/// <summary>
		/// The total amount of time the timer has spent paused.
		/// </summary>
		public float totalPauseTime { get; private set; }
		/// <summary>
		/// The time in which the timer was last paused.
		/// </summary>
		public float lastPauseTime { get; private set; }
		/// <summary>
		/// The current Timer or Stopwatch value.
		/// </summary>
		public float value { get; private set; }
		/// <summary>
		/// Returns a new TimeSpan object representing the current value.
		/// </summary>
		public TimeSpan span {
			get {
				return TimeSpan.FromSeconds(value);
			}
		}
		/// <summary>
		/// The current interval, returns 0 if <see cref="mechanism"/>
		/// == Timer.
		/// </summary>
		public uint interval {
			get {
				return mechanism == Mechanism.Timer ? 0 : nextInterval - 1;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// The target interval multiplier for a timer in Stopwatch mechanism.
		/// </summary>
		private uint nextInterval = 1;

		/// <summary>
		/// Creates and Resets a new timer in Timer Mechanism in Time TimeMode.
		/// </summary>
		public UTimer() : this(Mechanism.Timer, TimeMode.Time) { }

		/// <summary>
		/// Creates and Resets a new timer in Time TimeMode.
		/// </summary>
		/// <param name="mechanism">The directional mechanism to use.</param>
		public UTimer(Mechanism mechanism) : this(mechanism, TimeMode.Time) { }

		/// <summary>
		/// Creates and Resets a new timer in Timer Mechanism.
		/// </summary>
		/// <param name="timeMode">The time counting mode to use.</param>
		public UTimer(TimeMode timeMode) : this(Mechanism.Timer, timeMode) { }

		/// <summary>
		/// Creates and Resets a new timer.
		/// </summary>
		/// <param name="mode">The directional mechanism to use.</param>
		/// <param name="timeMode">The time counting mode to use.</param>
		public UTimer(Mechanism mode, TimeMode timeMode) {
			all.Add(this);
			this.mechanism = mode;
			this.timeMode = timeMode;
			Reset();
		}

		~UTimer() {
			all.Remove(this);
		}
		#endregion

		#region Controls
		/// <summary>
		/// This must be called by some external script for timers to update.
		/// </summary>
		public static void TickAll() {
			foreach(var t in all) {
				t.Tick();
			}
		}

		/// <summary>
		/// If currently stopped, resets and then starts the timer.
		/// </summary>
		public void Start() {
			if(state == State.Stopped) {
				Reset();
				startTime = TimeUtility.GetCurrentTime(timeMode);
				state = State.Running;
			}
		}

		/// <summary>
		/// If currently running, pauses the timer.
		/// </summary>
		public void Pause() {
			if(state == State.Running) {
				state = State.Paused;
				lastPauseTime = TimeUtility.GetCurrentTime(timeMode);
			}
		}

		/// <summary>
		/// If currently paused, resumes the timer.
		/// </summary>
		public void Resume() {
			if(state == State.Paused) {
				state = State.Running;
				totalPauseTime += TimeUtility.GetCurrentTime(timeMode) - lastPauseTime;
			}
		}

		/// <summary>
		/// If not currently stopped, stops the timer.
		/// </summary>
		public void Stop() {
			if(state != State.Stopped) {
				state = State.Stopped;
			}
		}

		/// <summary>
		/// Resets all readonly public and private timer
		/// counters.
		/// </summary>
		public void Reset() {
			state = State.Stopped;
			startTime = 0f;
			totalPauseTime = 0f;
			nextInterval = 1;
			hasElapsed = false;
		}

		/// <summary>
		/// Resets and starts the timer.
		/// </summary>
		public void Restart() {
			state = State.Stopped;
			Start();
		}

		/// <summary>
		/// Updates the timer's counters and values.
		/// </summary>
		private void Tick() {
			// Only update if running.
			if(state != State.Running) {
				return;
			}

			switch(mechanism) {
				case Mechanism.Timer:
					value = (duration - (TimeUtility.GetCurrentTime(timeMode) - startTime)) - totalPauseTime;
					if(value <= 0f) {
						FireElapsedEvent();
					}
					break;
				case Mechanism.Stopwatch:
					value = (TimeUtility.GetCurrentTime(timeMode) - startTime) - totalPauseTime;
					// To determine when to elapse the stopwatch, the target is determined by
					// way of using an interval counter.
					if(value >= duration * nextInterval) {
						nextInterval++;
						FireElapsedEvent();
					}
					break;
			}
		}
		#endregion

		private void FireElapsedEvent() {
			hasElapsed = true;
			if(TimerElapsed != null) {
				TimerElapsed();
			}
		}

		public override string ToString() {
			return span.ToString();
		}

		#region Enumerations
		/// <summary>
		/// Represents the current playback status of the timer.
		/// </summary>
		public enum State {
			Stopped,
			Running,
			Paused
		}

		public enum Mechanism {
			/// <summary>
			/// Counts down and elapses when <see cref="value"/> is less than or equal to 0.
			/// </summary>
			Timer,
			/// <summary>
			/// Counts up and elapses in intervals when <see cref="value"/>
			/// is greater than or equal to <see cref="nextInterval"/> * <see cref="duration"/>
			/// </summary>
			Stopwatch
		}
		#endregion
	}
}
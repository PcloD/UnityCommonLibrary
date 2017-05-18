using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityCommonLibrary.Time;
using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary.FSM
{
	/// <summary>
	/// A pushdown automata concurrent state machine.
	/// </summary>
	public sealed class UCLStateMachine<T> where T : struct, IFormattable, IConvertible, IComparable
	{
		/// <summary>
		/// Represents what the machine is currently doing.
		/// </summary>
		public enum Status
		{
			Stopped,
			InState,
			ExitingState,
			EnteringState
		}

		public delegate void OnLogFormat(string log);
		public delegate void OnStateSwitched(T previousState, T currentState);
		public delegate void OnStateEnter(T previous);
		public delegate void OnStateExit(T next);

		public static event OnLogFormat LogFormat;
		public event OnStateSwitched StateSwitched;

		public bool log;
		/// <summary>
		/// Queue of requested switches.
		/// </summary>
		private Queue<StateSwitch<T>> switchQueue = new Queue<StateSwitch<T>>();
		/// <summary>
		/// Represents the pushdown automata of the machine.
		/// Stores the history of switches to allow reversal.
		/// </summary>
		private Stack<T> history = new Stack<T>();
		private bool initialSwitch;
		private readonly Dictionary<T, bool> canTick = new Dictionary<T, bool>();
		private readonly Dictionary<T, OnStateEnter> onStateEnter = new Dictionary<T, OnStateEnter>();
		private readonly Dictionary<T, OnStateExit> onStateExit = new Dictionary<T, OnStateExit>();
		private readonly Dictionary<T, Action> onStateTick = new Dictionary<T, Action>();
		/// <summary>
		/// Creates a formatted string of detailed information
		/// about the current status of this machine.
		/// </summary>
		private readonly StringBuilder toStringBuilder = new StringBuilder();

		public TimeSlice lastSwitchTime { get; private set; }
		public T currentState { get; private set; }
		public T previousState { get; private set; }
		public Status status { get; private set; }
		public string id { get; private set; }
		public int historyCount
		{
			get
			{
				return history.Count;
			}
		}

		public UCLStateMachine(string id = null)
		{
			if(!typeof(T).IsEnum)
			{
				throw new Exception("T must be Enum");
			}
			this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
			for(int i = 0; i < EnumData<T>.Count; i++)
			{
				canTick.Add(EnumData<T>.values[i], true);
			}
		}

		public UCLStateMachine<T> SetOnEnter(T state, OnStateEnter onEnter)
		{
			onStateEnter.AddOrSet(state, onEnter);
			return this;
		}
		public UCLStateMachine<T> SetOnExit(T state, OnStateExit onExit)
		{
			onStateExit.AddOrSet(state, onExit);
			return this;
		}
		public UCLStateMachine<T> SetOnTick(T state, Action onTick)
		{
			onStateTick.AddOrSet(state, onTick);
			return this;
		}
		public void EngageMachine()
		{
			switch(status)
			{
				case Status.Stopped:
					initialSwitch = true;
					status = Status.InState;
					previousState = currentState = default(T);
					SwitchState(currentState);
					break;
			}
		}
		public void Tick()
		{
			switch(status)
			{
				case Status.InState:
					// Switch to next state if not switching
					if(switchQueue.Count > 0)
					{
						var nextSwitch = switchQueue.Dequeue();
						SwitchStateRoutine(nextSwitch);
					}
					else if(canTick[currentState])
					{
						Action tick;
						if(onStateTick.TryGetValue(currentState, out tick))
						{
							tick();
						}
					}
					break;
			}
		}
		public bool CanTick(T state)
		{
			return canTick[state];
		}
		public bool SetCanTick(T state, bool canTick)
		{
			return this.canTick[state] = canTick;
		}
		public bool IsInState(T state)
		{
			return currentState.Equals(state);
		}
		/// <summary>
		/// Rewinds to the previous state in history, if one exists.
		/// </summary>
		/// <remarks>
		/// This method exists because only nonreturning methods
		/// are allowed to be assigned to UnityEvents as callbacks
		/// in the Inspector
		/// </remarks>
		public void UEventRewind()
		{
			Rewind();
		}
		/// <summary>
		/// Rewinds to the previous state in history, if one exists.
		/// </summary>
		/// <returns>A StateSwitch object for further configuration, or null if history is empty.</returns>
		public StateSwitch<T> Rewind()
		{
			if(history.Count > 0)
			{
				return SwitchState(history.Pop(), StateSwitch<T>.Type.Rewind);
			}
			return null;
		}
		public StateSwitch<T> SwitchState(T state)
		{
			return SwitchState(state, StateSwitch<T>.Type.Switch);
		}
		/// <summary>
		/// Switches to the provided state instance.
		/// </summary>
		/// <param name="state">The state instance.</param>
		/// <param name="type">The kind of switch that will be performed.</param>
		/// <returns>A StateSwitch object for further configuration.</returns>
		private StateSwitch<T> SwitchState(T state, StateSwitch<T>.Type type)
		{
			if(!initialSwitch && currentState.Equals(state))
			{
				return null;
			}
			var stateSwitch = new StateSwitch<T>(state, type);
			switchQueue.Enqueue(stateSwitch);
			return stateSwitch;
		}
		/// <summary>
		/// The actual coroutine that switches states.
		/// </summary>
		/// <param name="switch">The StateSwitch instance to process.</param>
		private void SwitchStateRoutine(StateSwitch<T> @switch)
		{
			Log("Begin SwitchState type '{0}'", @switch.type);
			if(!initialSwitch)
			{
				Log("Exiting state '{0}'", currentState);
				// Exit current
				status = Status.ExitingState;

				OnStateExit onExit;
				if(onStateExit.TryGetValue(currentState, out onExit))
				{
					onExit(@switch.state);
				}
				// Only push the exiting state to history if not rewinding
				if(@switch.type == StateSwitch<T>.Type.Switch)
				{
					previousState = currentState;
					history.Push(currentState);
				}
			}

			var previous = currentState;
			// Enter next
			currentState = @switch.state;
			// Fire callback for onSwitch
			// TODO: Potentially add a new callback for pre/post switch callbacks
			@switch.FireOnSwitch();
			Log("Entering state '{0}'", currentState);
			status = Status.EnteringState;
			OnStateEnter onEnter;
			if(onStateEnter.TryGetValue(currentState, out onEnter))
			{
				onEnter(previous);
			}
			lastSwitchTime = TimeSlice.Create();
			status = Status.InState;
			if(!initialSwitch && StateSwitched != null)
			{
				StateSwitched(previousState, currentState);
			}
			initialSwitch = false;
		}
		private void StopCoroutines(params Coroutine[] routines)
		{
			for(int i = 0; i < routines.Length; i++)
			{
				if(routines[i] != null)
				{
					CoroutineUtility.StopCoroutine(routines[i]);
				}
			}
		}
		private void Log(string format, params object[] args)
		{
			if(log)
			{
				var str = string.Format("[{0}] {1}", id, string.Format(format, args));
				if(LogFormat == null)
				{
					Debug.Log(str);
				}
				else
				{
					LogFormat(str);
				}
			}
		}

		public override string ToString()
		{
			toStringBuilder.Length = 0;
			toStringBuilder.AppendLine(string.Format("ID: {0}", id));
			toStringBuilder.AppendLine(string.Format("Status: {0}", status));
			toStringBuilder.AppendLine(string.Format("PreviousState: {0}", previousState));
			toStringBuilder.AppendLine(string.Format("CurrentState: {0}", currentState));
			return toStringBuilder.ToString().Trim();
		}
	}
}
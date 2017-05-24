using System;
using System.Collections.Generic;
using System.Text;
using UnityCommonLibrary.Time;
using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary.FSM
{
    /// <summary>
    ///     A pushdown automata concurrent TimerState machine.
    /// </summary>
    public sealed class StateMachine<T>
        where T : struct, IFormattable, IConvertible, IComparable
    {
        /// <summary>
        ///     Represents what the machine is currently doing.
        /// </summary>
        public enum Status
        {
            Stopped,
            InState,
            ExitingState,
            EnteringState
        }

        public bool LogEvents;
        private readonly Dictionary<T, bool> _canTick = new Dictionary<T, bool>();

        /// <summary>
        ///     Represents the pushdown automata of the machine.
        ///     Stores the history of switches to allow reversal.
        /// </summary>
        private readonly Stack<T> _history = new Stack<T>();

        private readonly Dictionary<T, OnStateEnter> _onStateEnter =
            new Dictionary<T, OnStateEnter>();

        private readonly Dictionary<T, OnStateExit> _onStateExit =
            new Dictionary<T, OnStateExit>();

        private readonly Dictionary<T, Action> _onStateTick = new Dictionary<T, Action>();

        /// <summary>
        ///     Queue of requested switches.
        /// </summary>
        private readonly Queue<StateSwitch<T>> _switchQueue = new Queue<StateSwitch<T>>();

        /// <summary>
        ///     Creates a formatted string of detailed information
        ///     about the current MachineStatus of this machine.
        /// </summary>
        private readonly StringBuilder _toStringBuilder = new StringBuilder();

        private bool _initialSwitch;

        public delegate void OnLogFormat(string log);

        public delegate void OnStateEnter(T previous);

        public delegate void OnStateExit(T next);

        public delegate void OnStateSwitched(T previousState, T currentState);

        public static event OnLogFormat LogFormat;
        public event OnStateSwitched StateSwitched;
        public T CurrentState { get; private set; }

        public int HistoryCount
        {
            get { return _history.Count; }
        }

        public string Id { get; }

        public TimeSlice LastSwitchTime { get; private set; }
        public Status MachineStatus { get; private set; }
        public T PreviousState { get; private set; }

        public StateMachine(string id = null)
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be Enum");
            }
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            for (var i = 0; i < EnumData<T>.Count; i++)
            {
                _canTick.Add(EnumData<T>.Values[i], true);
            }
        }

        public bool CanTick(T state)
        {
            return _canTick[state];
        }

        public void EngageMachine()
        {
            switch (MachineStatus)
            {
                case Status.Stopped:
                    _initialSwitch = true;
                    MachineStatus = Status.InState;
                    PreviousState = CurrentState = default(T);
                    SwitchState(CurrentState);
                    break;
            }
        }

        public bool IsInState(T state)
        {
            return CurrentState.Equals(state);
        }

        /// <summary>
        ///     Rewinds to the previous TimerState in history, if one exists.
        /// </summary>
        /// <returns>A StateSwitch object for further configuration, or null if history is empty.</returns>
        public StateSwitch<T> Rewind()
        {
            if (_history.Count > 0)
            {
                return SwitchState(_history.Pop(), StateSwitch<T>.Type.Rewind);
            }
            return null;
        }

        public bool SetCanTick(T state, bool canTick)
        {
            return _canTick[state] = canTick;
        }

        public StateMachine<T> SetOnEnter(T state, OnStateEnter onEnter)
        {
            _onStateEnter.AddOrSet(state, onEnter);
            return this;
        }

        public StateMachine<T> SetOnExit(T state, OnStateExit onExit)
        {
            _onStateExit.AddOrSet(state, onExit);
            return this;
        }

        public StateMachine<T> SetOnTick(T state, Action onTick)
        {
            _onStateTick.AddOrSet(state, onTick);
            return this;
        }

        public StateSwitch<T> SwitchState(T state)
        {
            return SwitchState(state, StateSwitch<T>.Type.Switch);
        }

        public void Tick()
        {
            switch (MachineStatus)
            {
                case Status.InState:
                    // Switch to next TimerState if not switching
                    if (_switchQueue.Count > 0)
                    {
                        var nextSwitch = _switchQueue.Dequeue();
                        SwitchStateRoutine(nextSwitch);
                    }
                    else if (_canTick[CurrentState])
                    {
                        Action tick;
                        if (_onStateTick.TryGetValue(CurrentState, out tick))
                        {
                            tick();
                        }
                    }
                    break;
            }
        }

        public override string ToString()
        {
            _toStringBuilder.Length = 0;
            _toStringBuilder.AppendLine(string.Format("ID: {0}", Id));
            _toStringBuilder.AppendLine(string.Format("Status: {0}", MachineStatus));
            _toStringBuilder.AppendLine(
                string.Format("PreviousState: {0}", PreviousState));
            _toStringBuilder.AppendLine(string.Format("CurrentState: {0}", CurrentState));
            return _toStringBuilder.ToString().Trim();
        }

        /// <summary>
        ///     Rewinds to the previous TimerState in history, if one exists.
        /// </summary>
        /// <remarks>
        ///     This method exists because only nonreturning methods
        ///     are allowed to be assigned to UnityEvents as callbacks
        ///     in the Inspector
        /// </remarks>
        public void UEventRewind()
        {
            Rewind();
        }

        private void Log(string format, params object[] args)
        {
            if (!LogEvents)
            {
                return;
            }
            var str = string.Format("[{0}] {1}", Id, string.Format(format, args));
            if (LogFormat == null)
            {
                Debug.Log(str);
            }
            else
            {
                LogFormat(str);
            }
        }

        /// <summary>
        ///     Switches to the provided TimerState instance.
        /// </summary>
        /// <param name="state">The TimerState instance.</param>
        /// <param name="type">The kind of switch that will be performed.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        private StateSwitch<T> SwitchState(T state, StateSwitch<T>.Type type)
        {
            if (!_initialSwitch && CurrentState.Equals(state))
            {
                return null;
            }
            var stateSwitch = new StateSwitch<T>(state, type);
            _switchQueue.Enqueue(stateSwitch);
            return stateSwitch;
        }

        /// <summary>
        ///     The actual coroutine that switches states.
        /// </summary>
        /// <param name="switch">The StateSwitch instance to process.</param>
        private void SwitchStateRoutine(StateSwitch<T> @switch)
        {
            Log("Begin SwitchState StateType '{0}'", @switch.StateType);
            if (!_initialSwitch)
            {
                Log("Exiting TimerState '{0}'", CurrentState);
                // Exit current
                MachineStatus = Status.ExitingState;

                OnStateExit onExit;
                if (_onStateExit.TryGetValue(CurrentState, out onExit))
                {
                    onExit(@switch.State);
                }
                // Only push the exiting TimerState to history if not rewinding
                if (@switch.StateType == StateSwitch<T>.Type.Switch)
                {
                    PreviousState = CurrentState;
                    _history.Push(CurrentState);
                }
            }

            var previous = CurrentState;
            // Enter next
            CurrentState = @switch.State;
            // Fire callback for onSwitch
            // TODO: Potentially add a new callback for pre/post switch callbacks
            @switch.FireOnSwitch();
            Log("Entering TimerState '{0}'", CurrentState);
            MachineStatus = Status.EnteringState;
            OnStateEnter onEnter;
            if (_onStateEnter.TryGetValue(CurrentState, out onEnter))
            {
                onEnter(previous);
            }
            LastSwitchTime = TimeSlice.Create();
            MachineStatus = Status.InState;
            if (!_initialSwitch && StateSwitched != null)
            {
                StateSwitched(PreviousState, CurrentState);
            }
            _initialSwitch = false;
        }
    }
}
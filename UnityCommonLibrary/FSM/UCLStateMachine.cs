using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityCommonLibrary.Time;
using UnityCommonLibrary.Utilities;
using UnityEngine;

namespace UnityCommonLibrary.FSM
{
    /// <summary>
    /// A pushdown automata concurrent state machine.
    /// </summary>
    public sealed class UCLStateMachine<T> where T : struct, IFormattable, IConvertible, IComparable
    {
        public delegate void OnLogFormat(string log);
        public delegate void OnStateSwitched(T previousState, T currentState);
        public delegate void OnState(T previousState);
        public delegate IEnumerator OnStateAsync(T nextState);

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
        private Coroutine switchRoutine;
        private Coroutine enterRoutine;
        private Coroutine exitRoutine;
        private bool initialSwitch;
        private readonly Dictionary<T, OnState> onStateEnter = new Dictionary<T, OnState>();
        private readonly Dictionary<T, OnStateAsync> onStateEnterAsync = new Dictionary<T, OnStateAsync>();
        private readonly Dictionary<T, OnState> onStateExit = new Dictionary<T, OnState>();
        private readonly Dictionary<T, OnStateAsync> onStateExitAsync = new Dictionary<T, OnStateAsync>();
        private readonly Dictionary<T, Action> onStateTick = new Dictionary<T, Action>();
        private readonly T[] values;
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
            values = (T[])Enum.GetValues(typeof(T));
        }

        public UCLStateMachine<T> SetOnEnter(T state, OnState onEnter)
        {
            onStateEnter.AddOrSet(state, onEnter);
            return this;
        }
        public UCLStateMachine<T> SetOnEnterAsync(T state, OnStateAsync onEnter)
        {
            onStateEnterAsync.AddOrSet(state, onEnter);
            return this;
        }
        public UCLStateMachine<T> SetOnExit(T state, OnState onExit)
        {
            onStateExit.AddOrSet(state, onExit);
            return this;
        }
        public UCLStateMachine<T> SetOnExitAsync(T state, OnStateAsync onExit)
        {
            onStateExitAsync.AddOrSet(state, onExit);
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
                        StopCoroutines(switchRoutine, enterRoutine, exitRoutine);
                        switchRoutine = CoroutineUtility.StartCoroutine(SwitchStateRoutine(nextSwitch));
                    }
                    else
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
        private IEnumerator SwitchStateRoutine(StateSwitch<T> @switch)
        {
            Log("Begin SwitchState type '{0}'", @switch.type);
            if(!initialSwitch)
            {
                Log("Exiting state '{0}'", currentState);
                // Exit current
                status = Status.ExitingState;

                OnStateAsync onExitAsync;
                if(onStateExitAsync.TryGetValue(currentState, out onExitAsync))
                {
                    // Wait for exiting state to finish exiting.
                    exitRoutine = CoroutineUtility.StartCoroutine(onExitAsync(@switch.state));
                    yield return exitRoutine;
                    exitRoutine = null;
                }
                OnState onExit;
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

            // Enter next
            currentState = @switch.state;
            // Fire callback for onSwitch
            // TODO: Potentially add a new callback for pre/post switch callbacks
            @switch.FireOnSwitch();
            Log("Entering state '{0}'", currentState);
            status = Status.EnteringState;
            OnStateAsync onEnterAsync;
            if(onStateEnterAsync.TryGetValue(currentState, out onEnterAsync))
            {
                // Wait for exiting state to finish exiting.
                exitRoutine = CoroutineUtility.StartCoroutine(onEnterAsync(@switch.state));
                yield return exitRoutine;
                exitRoutine = null;
            }
            OnState onEnter;
            if(onStateEnter.TryGetValue(currentState, out onEnter))
            {
                onEnter(@switch.state);
            }
            lastSwitchTime = TimeSlice.Create();
            status = Status.InState;
            switchRoutine = null;
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
    }
}
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
    /// A hierarchial pushdown automata concurrent state machine. <para />
    ///	Hierarchial in that states == classes,
    ///	pushdown automata is implemented as a history stack, and
    ///	concurrent in that multiple state machines are permitted
    ///	on the same GameObject and are uniquely indentifiable.
    /// </summary>
    public sealed class HPDAStateMachine
    {
        private static readonly Dictionary<int, HPDAStateMachine> allMachines = new Dictionary<int, HPDAStateMachine>();

        public bool log;
        private Dictionary<int, AbstractHPDAState> states = new Dictionary<int, AbstractHPDAState>();
        /// <summary>
        /// Queue of requested switches.
        /// </summary>
        private Queue<StateSwitch> switchQueue = new Queue<StateSwitch>();
        /// <summary>
        /// Represents the pushdown automata of the machine.
        /// Stores the history of switches to allow reversal.
        /// </summary>
        private Stack<AbstractHPDAState> history = new Stack<AbstractHPDAState>();
        private Coroutine switchRoutine;
        private Coroutine enterRoutine;
        private Coroutine exitRoutine;

        public Status status { get; private set; }
        public AbstractHPDAState currentState { get; private set; }
        public AbstractHPDAState previousState { get; private set; }
        public string id { get; private set; }
        /// <summary>
        /// Exposes the number of states in the PDA history stack.
        /// </summary>
        public int historyCount {
            get {
                return history.Count;
            }
        }

        public HPDAStateMachine(string id = null)
        {
            this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            allMachines[Animator.StringToHash(this.id)] = this;
        }
        public HPDAStateMachine AddState(AbstractHPDAState state)
        {
            switch (status)
            {
                case Status.Stopped:
                    if (states.Count == 0)
                    {
                        previousState = state;
                    }
                    states.Add(state.GetHashCode(), state);
                    break;
            }
            return this;
        }
        public void EngageMachine()
        {
            switch (status)
            {
                case Status.Stopped:
                    if (states.Count == 0)
                    {
                        throw new Exception(string.Format("{0} states.Count == 0", id));
                    }
                    else
                    {
                        status = Status.InState;
                        var state = previousState;
                        previousState = currentState = null;
                        SwitchState(state.GetHashCode());
                    }
                    break;
            }
        }
        public void Tick()
        {
            switch (status)
            {
                case Status.InState:
                    // Switch to next state if not switching
                    if (switchQueue.Count > 0)
                    {
                        var nextSwitch = switchQueue.Dequeue();
                        StopCoroutines(switchRoutine, enterRoutine, exitRoutine);
                        switchRoutine = CoroutineUtility.StartCoroutine(SwitchStateRoutine(nextSwitch));
                    }
                    else
                    {
                        currentState.Tick();
                    }
                    break;
            }
        }

        #region State Switching
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
        public StateSwitch Rewind()
        {
            if (history.Count > 0)
            {
                return SwitchState(history.Pop(), StateSwitch.Type.Rewind);
            }
            return null;
        }
        /// <summary>
        /// Switches to the provided state instance.
        /// Mostly used by <see cref="UEventSwitchState"/>.
        /// state instances aren't normally used.
        /// For most switches in code, use <see cref="SwitchState{T}"/>.
        /// </summary>
        /// <param name="state">The instance to switch to.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        public StateSwitch SwitchState(string id)
        {
            return SwitchState(id, StateSwitch.Type.Switch);
        }
        /// <summary>
        /// Switches to the provided state instance.
        /// Mostly used by <see cref="UEventSwitchState"/>.
        /// state instances aren't normally used.
        /// For most switches in code, use <see cref="SwitchState{T}"/>.
        /// </summary>
        /// <param name="state">The instance to switch to.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        public StateSwitch SwitchState(string id, StateSwitch.Type type)
        {
            return SwitchState(Animator.StringToHash(id), type);
        }
        /// <summary>
        /// Switches to the provided state instance.
        /// Mostly used by <see cref="UEventSwitchState"/>.
        /// state instances aren't normally used.
        /// For most switches in code, use <see cref="SwitchState{T}"/>.
        /// </summary>
        /// <param name="state">The instance to switch to.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        public StateSwitch SwitchState(int hash)
        {
            return SwitchState(hash, StateSwitch.Type.Switch);
        }
        /// <summary>
        /// Switches to the provided state instance.
        /// </summary>
        /// <param name="state">The state instance.</param>
        /// <param name="type">The kind of switch that will be performed.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        public StateSwitch SwitchState(int hash, StateSwitch.Type type)
        {
            return SwitchState(states[hash], type);
        }
        /// <summary>
        /// Switches to the provided state instance.
        /// </summary>
        /// <param name="state">The state instance.</param>
        /// <param name="type">The kind of switch that will be performed.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        private StateSwitch SwitchState(AbstractHPDAState state, StateSwitch.Type type)
        {
            var stateSwitch = new StateSwitch(state, type);
            switchQueue.Enqueue(stateSwitch);
            return stateSwitch;
        }
        /// <summary>
        /// The actual coroutine that switches states.
        /// </summary>
        /// <param name="switch">The StateSwitch instance to process.</param>
        private IEnumerator SwitchStateRoutine(StateSwitch @switch)
        {
            Log("Begin SwitchState type '{0}'", @switch.type);
            if (currentState != null)
            {
                Log("Exiting state '{0}'", currentState.id);
                // Exit current
                status = Status.ExitingState;
                // Wait for exiting state to finish exiting.
                exitRoutine = CoroutineUtility.StartCoroutine(currentState.Exit(@switch.state));
                yield return exitRoutine;
                exitRoutine = null;
                // Only push the exiting state to history if not rewinding
                if (@switch.type == StateSwitch.Type.Switch)
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
            Log("Entering state '{0}'", currentState.id);
            status = Status.EnteringState;
            // Wait for entering state to finish entering.
            enterRoutine = CoroutineUtility.StartCoroutine(currentState.Enter(previousState));
            yield return enterRoutine;
            enterRoutine = null;
            currentState.timeEntered = TimeSlice.Create();
            status = Status.InState;
            switchRoutine = null;
        }
        private void StopCoroutines(params Coroutine[] routines)
        {
            for (int i = 0; i < routines.Length; i++)
            {
                if (routines[i] != null)
                {
                    CoroutineUtility.StopCoroutine(routines[i]);
                }
            }
        }
        private void Log(string format, params object[] args)
        {
            if (log)
            {
                Debug.LogFormat("[{0}] {1}", id, string.Format(format, args));
            }
        }
        #endregion

        #region Utility Methods
        public bool IsInState(int hash)
        {
            return currentState != null && currentState.GetHashCode() == hash;
        }

        /// <summary>
        /// Checks if the current state equals a state instance.
        /// Typically only used by Unity messages implemented in state classes as IsInState(this).
        /// For checking from one state to another, use <see cref="IsInState{T}"/>
        /// </summary>
        /// <param name="state">The state instance to check.</param>
        /// <returns>True if we are in this state, false otherwise.</returns>
        public bool IsInState(AbstractHPDAState state)
        {
            return currentState == state;
        }
        public static void EngageAllMachines()
        {
            foreach (var kvp in allMachines)
            {
                kvp.Value.EngageMachine();
            }
        }
        #endregion

        /// <summary>
        /// Creates a formatted string of detailed information
        /// about the current status of this machine.
        /// </summary>
        private readonly StringBuilder toStringBuilder = new StringBuilder();
        public override string ToString()
        {
            toStringBuilder.Length = 0;
            toStringBuilder.AppendLine(string.Format("ID: {0}", id));
            toStringBuilder.AppendLine(string.Format("Status: {0}", status));
            toStringBuilder.AppendLine(string.Format("CurrentState: {0}", currentState.id));
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
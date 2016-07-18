using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private static readonly Dictionary<string, HPDAStateMachine> allMachines = new Dictionary<string, HPDAStateMachine>();

        public ReadOnlyCollection<AbstractHPDAState> readonlyStates { get; private set; }
        public Status activity { get; private set; }
        public AbstractHPDAState currentState { get; private set; }
        public AbstractHPDAState previousState { get; private set; }
        private List<AbstractHPDAState> states = new List<AbstractHPDAState>();
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

        public string id { get; private set; }
        /// <summary>
        /// Exposes the number of states in the PDA history stack.
        /// </summary>
        public int historyCount
        {
            get
            {
                return history.Count;
            }
        }

        public HPDAStateMachine(string id = null)
        {
            states = new List<AbstractHPDAState>();
            this.id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            allMachines[this.id] = this;
            readonlyStates = new ReadOnlyCollection<AbstractHPDAState>(states);
        }
        public HPDAStateMachine AddState(AbstractHPDAState state)
        {
            switch (activity)
            {
                case Status.Stopped:
                    states.Add(state);
                    break;
            }
            return this;
        }
        public void EngageMachine()
        {
            switch (activity)
            {
                case Status.Stopped:
                    if (states.Count == 0)
                    {
                        throw new Exception(string.Format("{0} states.Count == 0", id));
                    }
                    else
                    {
                        activity = Status.InState;
                        SwitchState(states[0]);
                    }
                    break;
            }
        }
        public void Tick()
        {
            switch (activity)
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
        public StateSwitch SwitchState(AbstractHPDAState state)
        {
            return SwitchState(state, StateSwitch.Type.Switch);
        }
        /// <summary>
        /// Switches to the provided state instance.
        /// </summary>
        /// <param name="state">The state instance.</param>
        /// <param name="type">The kind of switch that will be performed.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        public StateSwitch SwitchState(AbstractHPDAState state, StateSwitch.Type type)
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
            if (currentState != null)
            {
                // Exit current
                activity = Status.ExitingState;
                // Wait for exiting state to finish exiting.
                exitRoutine = CoroutineUtility.StartCoroutine(currentState.Exit());
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
            activity = Status.EnteringState;
            // Wait for entering state to finish entering.
            enterRoutine = CoroutineUtility.StartCoroutine(currentState.Enter());
            yield return enterRoutine;
            enterRoutine = null;
            currentState.timeEntered = TimeSlice.Create();
            activity = Status.InState;
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
        #endregion

        #region Utility Methods
        /// <summary>
        /// Checks if the current state type equals a state class type.
        /// </summary>
        /// <typeparam name="T">The state class type to check.</typeparam>
        /// <returns>True if we are in this state, false otherwise.</returns>
        public bool IsInState<T>() where T : AbstractHPDAState
        {
            return currentState.GetType() == typeof(T);
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
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Status:");
            sb.Append('\t');
            sb.AppendLine(activity.ToString());
            sb.AppendLine("CurrentState:");
            sb.Append('\t');
            sb.AppendLine(currentState.id);
            if (history.Count > 0)
            {
                sb.AppendLine("History (Last 10):");
                foreach (var s in history.Take(10))
                {
                    sb.Append('\t');
                    sb.AppendLine(s.GetType().Name);
                }
            }
            return sb.ToString().Trim();
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
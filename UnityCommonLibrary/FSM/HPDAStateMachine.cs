using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityCommonLibrary.Time;
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
    public sealed class HPDAStateMachine : MonoBehaviour
    {
        #region Inspector Exposed Fields
        /// <summary>
        /// A unique ID to differentiate between multiple
        /// HPDAStateMachine instances on the same GameObject.
        /// </summary>
        [SerializeField]
        private string _id;
        /// <summary>
        /// If true, states won't be disabled when they exit.
        /// </summary>
        [SerializeField]
        private bool statesAlwaysEnabled;
        /// <summary>
        /// The states that can be transitioned to in this machine.
        /// </summary>
        [SerializeField]
        private List<HPDAState> states = new List<HPDAState>();
        #endregion

        #region Properties
        /// <summary>
        /// A unique ID to differentiate between multiple
        /// HPDAStateMachine instances on the same GameObject.
        /// </summary>
        public string id { get { return _id; } }
        /// <summary>
        /// Endpoint for public readonly access to machine states.
        /// </summary>
        public ReadOnlyCollection<HPDAState> readonlyStates { get; private set; }
        /// <summary>
        /// Represents what the machine is currently doing.
        /// </summary>
        public Status activity { get; private set; }
        /// <summary>
        /// The active state of the machine.
        /// </summary>
        public HPDAState currentState { get; private set; }
        public HPDAState previousState { get; private set; }

        /// <summary>
        /// Exposes the number of states in the PDA history stack.
        /// </summary>
        public int historyCount {
            get {
                return history.Count;
            }
        }

        #endregion

        #region Private Fields
        private static readonly Dictionary<string, HPDAStateMachine> all = new Dictionary<string, HPDAStateMachine>();
        /// <summary>
        /// Queue of requested switches.
        /// </summary>
        private Queue<StateSwitch> switchQueue = new Queue<StateSwitch>();
        /// <summary>
        /// Represents the pushdown automata of the machine.
        /// Stores the history of switches to allow reversal.
        /// </summary>
        private Stack<HPDAState> history = new Stack<HPDAState>();
        #endregion

        public static Dictionary<string, HPDAStateMachine>.KeyCollection allStateIDs {
            get {
                return all.Keys;
            }
        }
        public static Dictionary<string, HPDAStateMachine>.ValueCollection allStates {
            get {
                return all.Values;
            }
        }

        #region Unity Messages
        private void Awake()
        {
            if (all.ContainsKey(id))
            {
                Debug.LogErrorFormat(this, "HPDAStateMachine registry already contains machine with id '{0}'", id);
                return;
            }
            all[id] = this;
        }
        private void Start()
        {
            readonlyStates = new ReadOnlyCollection<HPDAState>(states);
            // Register each state with this machine
            foreach (var s in states)
            {
                s.Register(this);
            }

            // NullObject pattern, register NullState to begin with
            // Not actually in state list
            currentState = gameObject.AddComponent<NullState>();
            currentState.hideFlags = HideFlags.HideAndDontSave;

            if (states.Count == 0)
            {
                Debug.LogError("states.Count == 0!", this);
            }
            else {
                previousState = states[0];
                SwitchState(previousState);
            }
        }
        private void Update()
        {
            // Switch to next state if not switching
            if (activity == Status.InState && switchQueue.Count > 0)
            {
                var nextSwitch = switchQueue.Dequeue();
                // Not really necessary, but just in case
                StopAllCoroutines();
                StartCoroutine(SwitchStateRoutine(nextSwitch));
            }
            else {
                currentState.UpdateState();
            }
        }
        private void Reset()
        {
            // Generate new random ID
            _id = "StateMachine_" + Guid.NewGuid().ToString().Substring(0, 4);
        }
        #endregion

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
        /// Switches to the provided state.
        /// </summary>
        /// <remarks>
        /// This method exists because only nonreturning methods
        /// are allowed to be assigned to UnityEvents as callbacks
        /// in the Inspector
        /// </remarks>
        public void UEventSwitchState(HPDAState state)
        {
            SwitchState(state);
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
        public StateSwitch SwitchState(HPDAState state)
        {
            return SwitchState(state, StateSwitch.Type.Switch);
        }
        /// <summary>
        /// Switches to state of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The class type of the state to switch to.</typeparam>
        /// <returns>A StateSwitch object for further configuration.</returns>
        public StateSwitch SwitchState<T>() where T : HPDAState
        {
            return SwitchState<T>(StateSwitch.Type.Switch);
        }
        /// <summary>
        /// Switches to state of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The class type of the state to switch to.</typeparam>
        /// <param name="type">The kind of switch that will be performed.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        private StateSwitch SwitchState<T>(StateSwitch.Type type) where T : HPDAState
        {
            var state = states.SingleOrDefault(s => s.GetType() == typeof(T));
            return SwitchState(state, type);
        }
        /// <summary>
        /// Switches to the provided state instance.
        /// </summary>
        /// <param name="state">The state instance.</param>
        /// <param name="type">The kind of switch that will be performed.</param>
        /// <returns>A StateSwitch object for further configuration.</returns>
        private StateSwitch SwitchState(HPDAState state, StateSwitch.Type type)
        {
            if (!state.canEnterState)
            {
                state.lastEnterFailure = TimeSlice.Create();
                return null;
            }
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
            // Exit current
            activity = Status.ExitingState;
            // Wait for exiting state to finish exiting.
            yield return StartCoroutine(currentState.Exit());
            // Only push the exiting state to history if not rewinding
            // and if the state is not the initial NullState
            if (!(currentState is NullState) && @switch.type == StateSwitch.Type.Switch)
            {
                previousState = currentState;
                history.Push(currentState);
            }
            // We don't want to mess with state enabled status unless
            // this flag is false.
            if (!statesAlwaysEnabled)
            {
                currentState.enabled = false;
            }

            // Enter next
            currentState = @switch.state;

            // Fire callback for onSwitch
            // TODO: Potentially add a new callback for pre/post switch callbacks
            if (@switch.onSwitch != null)
            {
                @switch.onSwitch();
            }
            activity = Status.EnteringState;
            // Wait for entering state to finish entering.
            yield return StartCoroutine(currentState.Enter());
            activity = Status.InState;
            // We always want to set the state to be enabled.
            currentState.enabled = true;
            currentState.stateEnterTime = TimeSlice.Create();
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Checks if the current state type equals a state class type.
        /// </summary>
        /// <typeparam name="T">The state class type to check.</typeparam>
        /// <returns>True if we are in this state, false otherwise.</returns>
        public bool IsInState<T>() where T : HPDAState
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
        public bool IsInState(HPDAState state)
        {
            return currentState == state;
        }
        public static HPDAStateMachine GetByID(string id)
        {
            HPDAStateMachine result = null;
            all.TryGetValue(id, out result);
            return result;
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
            sb.AppendLine(currentState.GetType().Name);
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
            InState,
            ExitingState,
            EnteringState
        }
    }

    /// <summary>
    /// Implementing the command design pattern,
    /// stores information about a requested switch.
    /// </summary>
    /// <remarks>
    /// TODO: Determine how we can use this class to pass information
    /// from one state to another on switch. Maybe look at using generics.
    /// </remarks>
    public class StateSwitch
    {
        public Action onSwitch;

        public readonly HPDAState state;
        public readonly Type type;

        public StateSwitch(HPDAState state, Type type)
        {
            this.state = state;
            this.type = type;
        }

        /// <summary>
        /// The type of switch to perform.
        /// </summary>
        public enum Type
        {
            Switch,
            Rewind
        }
    }
}
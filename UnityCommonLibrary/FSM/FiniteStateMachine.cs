using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCommonLibrary.FSM {
    [DisallowMultipleComponent]
    public sealed class FiniteStateMachine : MonoBehaviour {
        [SerializeField]
        private AbstractFSMState startState;

        private Queue<StateSwitch> switchQueue = new Queue<StateSwitch>();

        public Activity activity { get; private set; }
        public ReadOnlyCollection<AbstractFSMState> states { get; private set; }
        public AbstractFSMState currentState { get; private set; }

        private void Awake() {
            states = new ReadOnlyCollection<AbstractFSMState>(GetComponentsInChildren<AbstractFSMState>());
            foreach(var s in states) {
                s.Register(this);
            }
        }

        private void Start() {
            foreach(var s in states) {
                s.enabled = false;
                s.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
            }
            currentState = gameObject.AddComponent<NullState>();
            SwitchState(startState);
        }

        private void Update() {
            if(activity == Activity.InState && switchQueue.Count > 0) {
                var nextSwitch = switchQueue.Dequeue();
                StopAllCoroutines();
                StartCoroutine(SwitchStateRoutine(nextSwitch));
            }
        }

        private IEnumerator SwitchStateRoutine(StateSwitch @switch) {
            // Exit current
            activity = Activity.ExitingState;
            currentState.enabled = false;
            if(@switch.method != StateSwitch.Method.Overwrite) {
                var exit = currentState.Exit();
                if(exit != null) {
                    var coroutine = StartCoroutine(exit);
                    if(@switch.method == StateSwitch.Method.Ordered) {
                        yield return coroutine;
                    }
                }
            }
            currentState.ResetState();

            // Enter next
            activity = Activity.EnteringState;
            currentState = @switch.state;
            var enter = currentState.Enter();
            if(enter != null) {
                yield return StartCoroutine(enter);
            }
            currentState.enabled = true;
            activity = Activity.InState;
        }

        #region State Switching
        private bool CanSwitchToState(AbstractFSMState state) {
            return currentState != state;
        }

        public StateSwitch SwitchState<T>() where T : AbstractFSMState {
            return SwitchState<T>(StateSwitch.Method.Ordered);
        }

        public StateSwitch SwitchState<T>(StateSwitch.Method switchType) where T : AbstractFSMState {
            var state = states.SingleOrDefault(s => s.GetType() == typeof(T));
            return SwitchState(state, switchType);
        }

        public StateSwitch SwitchState(AbstractFSMState state) {
            return SwitchState(state, StateSwitch.Method.Ordered);
        }

        public StateSwitch SwitchState(AbstractFSMState state, StateSwitch.Method method) {
            if(!CanSwitchToState(state)) {
                return null;
            }
            var stateSwitch = new StateSwitch(state, method);
            switchQueue.Enqueue(stateSwitch);
            return stateSwitch;
        }

        /// <summary>
        /// Used with UnityEvents wired in Editor.
        /// </summary>
        public void SwitchStateOrdered(AbstractFSMState state) {
            SwitchState(state, StateSwitch.Method.Ordered);
        }

        /// <summary>
        /// Used with UnityEvents wired in Editor.
        /// </summary>
        public void SwitchStateCrossfade(AbstractFSMState state) {
            SwitchState(state, StateSwitch.Method.Crossfade);
        }

        /// <summary>
        /// Used with UnityEvents wired in Editor.
        /// </summary>
        public void SwitchStateOverwrite(AbstractFSMState state) {
            SwitchState(state, StateSwitch.Method.Overwrite);
        }
        #endregion

        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Activity: {0}", activity));
            sb.AppendLine(string.Format("CurrentState: {0}", currentState.GetType().Name));
            return sb.ToString().Trim();
        }

        public enum Activity {
            InState,
            ExitingState,
            EnteringState
        }

    }

    public class StateSwitch {
        public Action onSwitch;
        public Method method { get; private set; }
        public AbstractFSMState state { get; private set; }

        public StateSwitch(AbstractFSMState state) : this(state, Method.Ordered) { }

        public StateSwitch(AbstractFSMState state, Method switchType) {
            this.state = state;
            this.method = switchType;
        }

        public enum Method {
            Ordered,
            Crossfade,
            Overwrite
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityCommonLibrary.FSM {
    public sealed class HPDAStateMachine : MonoBehaviour {
        [SerializeField]
        private string _id;
        [SerializeField]
        private List<HPDAState> states = new List<HPDAState>();

        public string id { get { return _id; } }
        public ReadOnlyCollection<HPDAState> readonlyStates { get; private set; }
        public Activity activity { get; private set; }
        public HPDAState currentState { get; private set; }

        public int historyCount {
            get {
                return history.Count;
            }
        }

        private Queue<StateSwitch> switchQueue = new Queue<StateSwitch>();
        private Stack<HPDAState> history = new Stack<HPDAState>();

        #region Unity Methods
        private void Start() {
            readonlyStates = new ReadOnlyCollection<HPDAState>(states);
            foreach(var s in states) {
                s.Register(this);
            }

            currentState = gameObject.AddComponent<NullState>();
            currentState.hideFlags = HideFlags.HideAndDontSave;
            if(states.Count == 0) {
                Debug.LogError("states.Count == 0!", this);
            }
            else {
                SwitchState(states[0]);
            }
        }

        private void Update() {
            if(activity == Activity.InState && switchQueue.Count > 0) {
                var nextSwitch = switchQueue.Dequeue();
                StopAllCoroutines();
                StartCoroutine(SwitchStateRoutine(nextSwitch));
            }
            else {
                currentState.UpdateState();
            }
        }

        private void Reset() {
            _id = "StateMachine_" + Guid.NewGuid().ToString().Substring(0, 4);
        }
        #endregion

        #region State Switching
        public void UEventRewind() {
            Rewind();
        }

        public void UEventSwitchState(HPDAState state) {
            SwitchState(state);
        }

        public StateSwitch Rewind() {
            if(history.Count > 0) {
                return SwitchState(history.Pop(), StateSwitch.Type.Rewind);
            }
            return null;
        }

        public StateSwitch SwitchState(HPDAState state) {
            return SwitchState(state, StateSwitch.Type.None);
        }

        public StateSwitch SwitchState<T>() where T : HPDAState {
            return SwitchState<T>(StateSwitch.Type.None);
        }

        private StateSwitch SwitchState<T>(StateSwitch.Type type) where T : HPDAState {
            var state = states.SingleOrDefault(s => s.GetType() == typeof(T));
            return SwitchState(state, type);
        }

        private StateSwitch SwitchState(HPDAState state, StateSwitch.Type type) {
            var stateSwitch = new StateSwitch(state, type);
            switchQueue.Enqueue(stateSwitch);
            return stateSwitch;
        }

        private IEnumerator SwitchStateRoutine(StateSwitch @switch) {
            // Exit current
            activity = Activity.ExitingState;

            var exit = currentState.Exit();
            if(exit != null) {
                yield return StartCoroutine(exit);
            }
            if(!(currentState is NullState) && @switch.type == StateSwitch.Type.None) {
                history.Push(currentState);
            }
            currentState.enabled = false;

            // Enter next
            currentState = @switch.state;
            if(@switch.onSwitch != null) {
                @switch.onSwitch();
            }
            activity = Activity.EnteringState;
            var enter = currentState.Enter();
            if(enter != null) {
                yield return StartCoroutine(enter);
            }
            activity = Activity.InState;
            currentState.enabled = true;
        }
        #endregion

        public bool IsInState<T>() where T : HPDAState {
            return currentState.GetType() == typeof(T);
        }

        public bool IsInState(HPDAState state) {
            return currentState == state;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine("Activity:");
            sb.Append('\t');
            sb.AppendLine(activity.ToString());
            sb.AppendLine("CurrentState:");
            sb.Append('\t');
            sb.AppendLine(currentState.GetType().Name);
            if(history.Count > 0) {
                sb.AppendLine("History (Last 10):");
                foreach(var s in history.Take(10)) {
                    sb.Append('\t');
                    sb.AppendLine(s.GetType().Name);
                }
            }
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

        public readonly HPDAState state;
        public readonly Type type;

        public StateSwitch(HPDAState state, Type type) {
            this.state = state;
            this.type = type;
        }

        public enum Type {
            None,
            Rewind
        }
    }
}
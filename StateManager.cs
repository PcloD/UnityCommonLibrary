using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityCommonLibrary {
    public class StateManager : UCSingleton<StateManager>, IVisualDebuggable {

        public delegate void OnStateChanged(GameState previous, GameState next);

        public event OnStateChanged StatePostChanged;

        public float tMultiplier = 1f;

        public GameState state { get; set; }
        public GameState previous { get; set; }

        Status status = Status.Updating;

        public bool showDebug;

        void Awake() {
            VisualDebugger.get.RegisterFor(this);
            if(state == null && previous == null) {
                previous = state = new GameObject("EmptyState").AddComponent<EmptyState>();
            }
        }

        T FindState<T>() where T : GameState {
            var states = FindObjectsOfType<T>();
            if(states.Length == 0) {
                Debug.LogErrorFormat("STATE INSTANCE NOT FOUND: '{0}'", typeof(T).Name);
                return null;
            }
            else if(states.Length > 1) {
                Debug.LogErrorFormat("MULTIPLE STATE INSTANCES FOUND: '{0}'", typeof(T).Name);
                for(int i = 1; i < states.Length; i++) {
                    Destroy(states[i]);
                }
            }
            return states[0];
        }

        public void SwitchTo<T>() where T : GameState {
            status = Status.Exiting;
            StartCoroutine(_SwitchTo<T>());
        }

        IEnumerator _SwitchTo<T>() where T : GameState {
            var next = FindState<T>();

            if(!state.CheckExit(typeof(T))) {
                Debug.LogErrorFormat("CANNOT LEAVE CURRENT STATE: '{0}'", state);
            }
            else if(!next.CheckEnter(state.GetType())) {
                Debug.LogErrorFormat("CANNOT ENTER NEXT STATE '{0}'", next);
            }
            else {
                status = Status.Exiting;
                yield return StartCoroutine(state.ExitState());
                if(state.destroyOnExit) {
                    Destroy(state);
                }

                if(next is UIState) {
                    EventSystem.current.SetSelectedGameObject((next as UIState).firstSelected);
                }
                status = Status.Entering;
                yield return StartCoroutine(next.EnterState());

                previous = state;
                state = next;
                if(StatePostChanged != null) {
                    StatePostChanged(previous, next);
                }
            }
            status = Status.Updating;
        }

        void Update() {
            if(status == Status.Updating) {
                state.UpdateState();
            }
        }

        public void ShowDebugGUI() {
            GUILayout.Label("State Machine Status: " + status);
            GUILayout.Label("Previous State: " + previous.GetType().Name);
            GUILayout.Label("Current State: " + state.GetType().Name);
            GUILayout.Label("TimeSinceEnter: " + state.timeSinceEnter);
            GUILayout.Label("TimeSinceEnterScaled: " + state.timeSinceEnterScaled);
        }

        public enum Status {
            Updating,
            Entering,
            Exiting
        }

    }
}
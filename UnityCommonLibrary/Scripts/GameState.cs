using System;
using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary {
    public class EmptyState : GameState {

        protected override void Awake() {
            dontDestroyOnLoad = true;
            base.Awake();
        }
    }

    public abstract class GameState : UCScript {
        public bool dontDestroyOnLoad = true;

        public float tMultiplier {
            get { return StateManager.get.tMultiplier; }
            set { StateManager.get.tMultiplier = value; }
        }

        public float enterTime { get; private set; }
        public float enterTimeScaled { get; private set; }

        public float timeSinceEnter {
            get {
                return Time.unscaledTime - enterTime;
            }
        }
        public float timeSinceEnterScaled {
            get {
                return Time.time - enterTimeScaled;
            }
        }

        bool _destroyOnExit;
        public bool destroyOnExit {
            get { return _destroyOnExit; }
            set {
                if(value) {
                    _destroyOnExit = value;
                }
            }
        }

        protected virtual void Awake() {
            if(dontDestroyOnLoad) {
                DontDestroyOnLoad(this);
            }
        }

        internal IEnumerator EnterState() {
            enterTime = Time.unscaledTime;
            enterTimeScaled = Time.time;
            yield return StartCoroutine(Enter());
        }

        internal IEnumerator ExitState() {
            yield return StartCoroutine(Exit());
        }

        protected internal virtual bool CheckEnter(Type from) {
            return true;
        }

        protected internal virtual bool CheckExit(Type to) {
            return true;
        }

        protected internal virtual void UpdateState() { }

        protected virtual IEnumerator Enter() {
            yield return null;
        }

        protected virtual IEnumerator Exit() {
            yield return null;
        }

        public override string ToString() {
            return GetType().Name;
        }

        public void MarkForDeletion() {
            destroyOnExit = true;
        }

        protected bool IsOneOf(Type check, params Type[] against) {
            return Array.IndexOf(against, check) >= 0;
        }
    }
}
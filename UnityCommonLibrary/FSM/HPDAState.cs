using System;
using System.Collections;
using UnityCommonLibrary.Utilities;
using UnityEngine;

namespace UnityCommonLibrary.FSM {
    public abstract class HPDAState : MonoBehaviour {
        protected HPDAStateMachine machine { get; private set; }
        public bool isStateActive {
            get {
                return machine.IsInState(this);
            }
        }

        public void Register(HPDAStateMachine machine) {
            if(this.machine == null) {
                this.machine = machine;
            }
        }

        public virtual IEnumerator Enter() { return null; }
        public virtual IEnumerator Exit() { return null; }

        public virtual void Initialize() { }
        public virtual void UpdateState() { }
        public virtual void ResetState() { }
    }
}

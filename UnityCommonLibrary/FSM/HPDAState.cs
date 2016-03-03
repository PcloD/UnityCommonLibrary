using System.Collections;
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

        public virtual IEnumerator Enter() { yield break; }
        public virtual IEnumerator Exit() { yield break; }
        public virtual void UpdateState() { }
    }
}

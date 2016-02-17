using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary.FSM {
    public abstract class AbstractFSMState : MonoBehaviour {
        protected FiniteStateMachine fsm { get; private set; }
        public void Register(FiniteStateMachine fsm) {
            if(this.fsm == null) {
                this.fsm = fsm;
            }
        }

        public virtual IEnumerator Enter() { return null; }
        public virtual IEnumerator Exit() { return null; }

        public virtual void ResetState() { }
    }
}

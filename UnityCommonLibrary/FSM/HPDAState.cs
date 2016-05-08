using System;
using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary.FSM
{
    /// <summary>
    /// Represents a single state in a <see cref="HPDAStateMachine"/>
    /// </summary>
    public abstract class HPDAState : MonoBehaviour
    {
        /// <summary>
        /// The machine that manages this state.
        /// </summary>
        public HPDAStateMachine machine { get; private set; }
        /// <summary>
        /// Utility property for <see cref="HPDAStateMachine.IsInState(HPDAState)"/>
        /// </summary>
        public bool isCurrentState {
            get {
                return machine.IsInState(this);
            }
        }

        public virtual bool canTransitionFromAny {
            get {
                return false;
            }
        }

        /// <summary>
        /// Permits a one-time registration with a state machine.
        /// </summary>
        /// <param name="machine">The machine to call home.</param>
        public void Register(HPDAStateMachine machine)
        {
            if (this.machine == null)
            {
                this.machine = machine;
            }
        }
        /// <summary>
        /// Coroutine used to enter this state.
        /// By default performs a <code>yield break;</code>
        /// </summary>
        public virtual IEnumerator Enter() { yield break; }
        /// <summary>
        /// Coroutine used to exit this state.
        /// By default performs a <code>yield break;</code>
        /// </summary>
        public virtual IEnumerator Exit() { yield break; }
        /// <summary>
        /// Called by the owning machine to control when
        /// Updating occurs (when state is active).
        /// </summary>
        public virtual void UpdateState() { }
        public virtual bool CanTransitionTo(Type stateType) { return false; }
    }
}

using System;

namespace UnityCommonLibrary.FSM
{
    /// <summary>
    /// Implementation of the null object pattern.
    /// Ensures that a <see cref="HPDAStateMachine"/>
    /// can always start up.
    /// </summary>
    public sealed class NullState : HPDAState
    {

        public override bool CanTransitionTo(Type stateType)
        {
            return true;
        }
    }
}
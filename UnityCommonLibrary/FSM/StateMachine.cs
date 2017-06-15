using System;
using System.Collections.Generic;
using UnityCommonLibrary.Time;

namespace UnityCommonLibrary.FSM
{
    public sealed class StateMachine<T> :
        BaseStateMachine<T, StateMachine<T>.OnEnter, StateMachine<T>.OnExit>
        where T : struct, IFormattable, IConvertible, IComparable
    {
        public delegate void OnEnter(T previousState);
        public delegate void OnExit(T nextState);

        public void ChangeState(T nextState)
        {
            if (Equals(nextState, CurrentState))
            {
                return;
            }
            HashSet<OnExit> exitCallbacks;
            if (OnStateExit.TryGetValue(CurrentState, out exitCallbacks))
            {
                foreach (var callback in exitCallbacks)
                {
                    callback(nextState);
                }
            }
            PreviousState = CurrentState;
            CurrentState = nextState;
            HashSet<OnEnter> enterCallbacks;
            if (OnStateEnter.TryGetValue(CurrentState, out enterCallbacks))
            {
                foreach (var callback in enterCallbacks)
                {
                   callback(PreviousState);
                }
            }
            StateEnterTime = TimeSlice.Create();
            StateChanged.Publish(PreviousState, CurrentState);
        }

        /// <inheritdoc />
        public override void RemoveCallbacks(object obj)
        {
            foreach (var list in OnStateEnter.Values)
            {
                list.RemoveWhere(m => m.Target == obj);
            }
            foreach (var list in OnStateExit.Values)
            {
                list.RemoveWhere(m => m.Target == obj);
            }
        }
    }
}
using System;

namespace UnityCommonLibrary.FSM
{
    /// <summary>
    ///     Implementing the command design pattern,
    ///     stores information about a requested switch.
    /// </summary>
    /// <remarks>
    ///     TODO: Determine how we can use this class to pass information
    ///     from one TimerState to another on switch. Maybe look at using generics.
    /// </remarks>
    public class StateSwitch<T> where T : struct, IFormattable, IConvertible, IComparable
    {
        /// <summary>
        ///     The StateType of switch to perform.
        /// </summary>
        public enum Type
        {
            Switch,
            Rewind
        }

        public delegate void OnSwitch();

        public event OnSwitch Switch;

        public T State { get; }

        public Type StateType { get; }

        public StateSwitch(T state, Type stateType)
        {
            State = state;
            StateType = stateType;
        }

        internal void FireOnSwitch()
        {
            if (Switch != null)
            {
                Switch();
            }
        }
    }
}
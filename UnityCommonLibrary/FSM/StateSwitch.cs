namespace UnityCommonLibrary.FSM
{
    /// <summary>
    /// Implementing the command design pattern,
    /// stores information about a requested switch.
    /// </summary>
    /// <remarks>
    /// TODO: Determine how we can use this class to pass information
    /// from one state to another on switch. Maybe look at using generics.
    /// </remarks>
    public class StateSwitch
    {
        public delegate void OnSwitchEvent();
        public event OnSwitchEvent onSwitch;

        private readonly AbstractHPDAState _state;
        private readonly Type _type;

        public AbstractHPDAState state
        {
            get
            {
                return _state;
            }
        }
        public Type type
        {
            get
            {
                return _type;
            }
        }

        public StateSwitch(AbstractHPDAState state, Type type)
        {
            _state = state;
            _type = type;
        }

        /// <summary>
        /// The type of switch to perform.
        /// </summary>
        public enum Type
        {
            Switch,
            Rewind
        }

        internal void FireOnSwitch()
        {
            if (onSwitch != null)
            {
                onSwitch();
            }
        }
    }
}

using System;

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
	public class StateSwitch<T> where T : struct, IFormattable, IConvertible, IComparable
	{
		public delegate void OnSwitch();
		public event OnSwitch Switch;

		private readonly T _state;
		private readonly Type _type;

		public T state
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

		public StateSwitch(T state, Type type)
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
			if(Switch != null)
			{
				Switch();
			}
		}
	}
}
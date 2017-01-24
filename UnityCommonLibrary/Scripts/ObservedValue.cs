namespace UnityCommonLibrary
{
	public class ObservedValue<T>
	{
		public delegate void OnValueChanged(T oldValue, T newValue);
		public event OnValueChanged ValueChanged;

		private T previousValue;
		private T internalValue;

		public T value
		{
			get
			{
				return internalValue;
			}
			set
			{
				var didChange = (internalValue != null && value == null) ||
								(internalValue == null && value != null) ||
								(internalValue.Equals(value) == false);
				if(didChange && ValueChanged != null)
				{
					previousValue = internalValue;
					internalValue = value;
					ValueChanged(previousValue, internalValue);
				}
			}
		}

		public ObservedValue() : this(default(T)) { }
		public ObservedValue(T value)
		{
			previousValue = internalValue = value;
		}

		public static implicit operator T(ObservedValue<T> t)
		{
			return t.value;
		}
	}
}
namespace UnityCommonLibrary
{
    public class ObservedValue<T>
    {
        private T _value;

        public delegate void OnValueChanged(T oldValue, T newValue);

        public event OnValueChanged ValueChanged;

        public T Value
        {
            get { return _value; }
            set
            {
                if (!Equals(_value, value))
                {
                    var previousValue = _value;
                    _value = value;
                    if (ValueChanged != null)
                    {
                        ValueChanged(previousValue, _value);
                    }
                }
            }
        }

        public ObservedValue() : this(default(T)) { }

        public ObservedValue(T value)
        {
            _value = value;
        }

        public static implicit operator T(ObservedValue<T> t)
        {
            return t.Value;
        }
    }
}
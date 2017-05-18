namespace UnityCommonLibrary
{
    public class ObservedValue<T>
    {
        public delegate void OnValueChanged(T oldValue, T newValue);
        public event OnValueChanged ValueChanged;

        private T value;

        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                if (!Equals(this.value, value))
                {
                    var previousValue = this.value;
                    this.value = value;
                    if (ValueChanged != null)
                    {
                        ValueChanged(previousValue, this.value);
                    }
                }
            }
        }

        public ObservedValue() : this(default(T)) { }
        public ObservedValue(T value)
        {
            this.value = value;
        }

        public static implicit operator T(ObservedValue<T> t)
        {
            return t.Value;
        }
    }
}
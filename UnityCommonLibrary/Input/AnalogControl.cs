using UInput = UnityEngine.Input;

namespace UnityCommonLibrary.Input
{
    public sealed class AnalogControl : Control
    {
        public string axisName = string.Empty;
        public float value { get; private set; }
        public float rawValue { get; private set; }

        public AnalogControl(string name) : base(name) { }

        public AnalogControl(string name, string axisName) : base(name)
        {
            this.axisName = axisName;
        }

        internal override void Update()
        {
            value = UInput.GetAxis(axisName);
            rawValue = UInput.GetAxisRaw(axisName);
        }

        public override void Reset()
        {
            rawValue = 0f;
            value = 0f;
        }
    }
}

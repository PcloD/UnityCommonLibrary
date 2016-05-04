using UnityEngine;
using UInput = UnityEngine.Input;

namespace UnityCommonLibrary.Input
{
    public sealed class DigitalControl : Control {
        public KeyCode code = KeyCode.None;
        public ControlState state { get; private set; }

        public DigitalControl(string name) : base(name) { }

        public DigitalControl(string name, KeyCode code) : base(name) {
            this.code = code;
        }

        public bool InState(ControlState state) {
            return (this.state & state) != 0;
        }

        internal override void Update() {
            if(UInput.GetKey(code)) {
                state |= ControlState.Held;
            }
            if(UInput.GetKeyDown(code)) {
                state |= ControlState.Down;
            }
            if(UInput.GetKeyUp(code)) {
                state |= ControlState.Up;
            }
        }

        public override void Reset() {
            state = ControlState.None;
        }
    }
}

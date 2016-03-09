using System.Collections.Generic;

namespace UnityCommonLibrary.Input {
    public class InputWrapper : MonoSingleton<InputWrapper> {
        private static readonly AnalogControl NULL_ANALOG = new AnalogControl("NULL_ANALOG");
        private static readonly DigitalControl NULL_DIGITAL = new DigitalControl("NULL_DIGITAL");

        private readonly Dictionary<string, Control> controls = new Dictionary<string, Control>();
        public bool isEnabled = true;

        private void Update() {
            foreach(var c in controls.Values) {
                c.Reset();
                if(isEnabled) {
                    c.Update();
                }
            }
        }

        public void RegisterControls(params Control[] controls) {
            foreach(var c in controls) {
                RegisterControl(c);
            }
        }

        public void RegisterControl(Control c) {
            controls[c.name] = c;
        }

        public AnalogControl GetAnalog(string name) {
            Control control;
            if(controls.TryGetValue(name, out control)) {
                if(control is AnalogControl) {
                    return (AnalogControl)control;
                }
            }
            return NULL_ANALOG;
        }

        public DigitalControl GetDigital(string name) {
            Control control;
            if(controls.TryGetValue(name, out control)) {
                if(control is DigitalControl) {
                    return (DigitalControl)control;
                }
            }
            return NULL_DIGITAL;
        }
    }
}

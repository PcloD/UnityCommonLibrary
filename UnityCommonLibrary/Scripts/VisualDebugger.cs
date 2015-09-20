using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityCommonLibrary {
    public class VisualDebugger : UCSingleton<VisualDebugger> {
        KeyCode toggle = KeyCode.F1;
        List<DebugElement> elements = new List<DebugElement>();
        Vector2 scroll;
        public bool visible { get; private set; }

        public void RegisterFor(IVisualDebuggable target) {
            RegisterFor(target, target.GetType().Name);
        }

        public void RegisterFor(IVisualDebuggable target, string header) {
            var exists = elements.FirstOrDefault(e => e.target == target);
            if(exists != null) {
                Debug.LogErrorFormat("TARGET ALREADY EXISTS! {0} was to replace {1}", target.GetType().Name, exists.target.GetType().Name);
                return;
            }
            elements.Add(new DebugElement(target, header));
        }

        void Update() {
            visible = Input.GetKeyDown(toggle) ? !visible : visible;
        }

        void OnGUI() {
            if(!visible) {
                return;
            }
            GUILayout.BeginHorizontal();
            for(var i = 0; i < elements.Count; i++) {
                var e = elements[i];
                if(e.target == null) {
                    elements.RemoveAt(i);
                    continue;
                }

                GUILayout.BeginVertical(GUI.skin.box);
                GUILayout.Label(string.Format(RichText.MakeBold("{0} [{1}]"), e.header, (e.target as Object).GetInstanceID()));
                DrawReflectionGUI(e.target);
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawReflectionGUI(IVisualDebuggable target) {
            //Show fields
            var fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).ToArray();
            var values = fields.Select(f => f.GetValue(target)).ToArray();
            for(var i = 0; i < fields.Length; i++) {
                var f = fields[i];
                var val = values[i];
                var valStr = val == null ? "null" : val.ToString();
                valStr = RichText.MakeBold(valStr);
                if(f.Name.Contains("k__BackingField")) {
                    continue;
                }
                GUILayout.Label(string.Format("{0}: {1}", f.Name, valStr));
            }

            //Show properties
            var props = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).ToArray();
            values = props.Select(p => p.GetValue(target, null)).ToArray();
            for(var i = 0; i < props.Length; i++) {
                var p = props[i];
                var val = values[i];
                var valStr = val == null ? "null" : val.ToString();
                valStr = RichText.MakeBold(valStr);
                GUILayout.Label(string.Format("{0}: {1}", p.Name, valStr));
            }
        }

        private string BeautifyFieldName(string name) {
            if(name == "Single") {
                return "float";
            }
            else if(name == "Boolean") {
                return "bool";
            }
            else if(name == "Int32") {
                return "int";
            }
            else if(name == "Int16") {
                return "short";
            }
            else if(name == "Byte" || name == "SByte" || name == "Char" || name == "Double" || name == "String") {
                return name.ToLower();
            }
            else {
                return name;
            }
        }
    }

    public class DebugElement {
        public IVisualDebuggable target { get; private set; }
        public string header { get; private set; }

        public DebugElement(IVisualDebuggable target, string header) {
            this.target = target;
            this.header = header;
        }
    }

    public interface IVisualDebuggable { }

    public static class RichText {
        public static string MakeBold(string s) {
            return string.Format("<b>{0}</b>", s);
        }

        public static string MakeItalicized(string s) {
            return string.Format("<i>{0}</i>", s);
        }

        public static string MakeColored(string s, Color color) {
            return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGBA(color), s);
        }

        public static string MakeSized(string s, int size) {
            return string.Format("<size={0}>{1}</size>", size, s);
        }
    }
}
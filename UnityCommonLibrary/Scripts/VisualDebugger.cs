using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;

namespace UnityCommonLibrary {
    public class VisualDebugger : UCSingleton<VisualDebugger> {
        KeyCode toggle = KeyCode.F1;
        List<DebugElement> elements = new List<DebugElement>();
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
                GUILayout.Label(string.Format("<b>{0}</b>", e.header));
                GUILayout.Label(string.Format("{0}", (e.target as UnityEngine.Object).GetInstanceID()));
                e.target.ShowDebugGUI();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
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

    public interface IVisualDebuggable {
        void ShowDebugGUI();
    }

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
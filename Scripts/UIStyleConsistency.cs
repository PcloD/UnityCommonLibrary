using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public class UIStyleConsistency : UCScript {

        [SerializeField, Header("Options")]
        bool sceneWide = false;

        [SerializeField]
        bool keepInBuilds;

        [SerializeField]
        Component[] ignoreList;

        [SerializeField, Header("Colors")]
        Color globalTextColor;

        [SerializeField]
        Color buttonTextColor = Color.white,
              buttonGraphicColor = Color.white,
              inputFieldColor = Color.white,
              inputFieldTextColor = Color.white;

        [SerializeField, Header("Fonts")]
        Font globalFont;

        [SerializeField]
        Font buttonFont;

        [SerializeField, Header("Selectables")]
        ColorBlock selectableStyles;

        void Awake() {
            hideFlags = HideFlags.DontSaveInBuild;
        }

        void Update() {
            hideFlags = keepInBuilds ? HideFlags.None : HideFlags.DontSaveInBuild;
            var selectables = FindObjectsOfType<Selectable>().Where(s => !ignoreList.Contains(s));
            var texts = FindObjectsOfType<Text>().Where(t => !ignoreList.Contains(t));
            var buttons = FindObjectsOfType<Button>().Where(b => !ignoreList.Contains(b));
            var inputFields = FindObjectsOfType<InputField>().Where(i => !ignoreList.Contains(i));

            foreach(var s in selectables) {
                if(sceneWide == false && s.transform.root != gameObject.transform)
                    continue;

                s.colors = selectableStyles;
            }
            foreach(var t in texts) {
                if(sceneWide == false && t.transform.root != gameObject.transform)
                    continue;

                t.color = (t.GetComponentInParent<InputField>() == null) ? globalTextColor : inputFieldTextColor;
                t.font = globalFont;
            }
            foreach(var i in inputFields) {
                if(sceneWide == false && i.transform.root != gameObject.transform)
                    continue;

                i.targetGraphic.color = inputFieldColor;
            }
            foreach(var b in buttons) {
                if(sceneWide == false && b.transform.root != gameObject.transform)
                    continue;

                b.colors = selectableStyles;
                b.targetGraphic.color = buttonGraphicColor;
                var bText = b.GetComponentInChildren<Text>();
                if(bText != null) {
                    bText.color = buttonTextColor;
                    bText.font = buttonFont == null ? globalFont : buttonFont;
                }
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public class UIStyleConsistency : UCScript {
        [SerializeField, Header("Selectables")]
        ColorBlock selectable;

        void Update() {
            var selectables = Selectable.allSelectables;
            foreach(var s in selectables) {
                s.colors = selectable;
            }
        }

        void Reset() {
            selectable = ColorBlock.defaultColorBlock;
        }
    }
}
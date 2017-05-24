using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary.UI
{
    [ExecuteInEditMode]
    public class UiStyleConsistency : MonoBehaviour
    {
        [SerializeField]
        [Header("Font")]
        private Font _font;

        [SerializeField]
        [Header("Selectables")]
        private ColorBlock _selectable;

        public void FullUpdate()
        {
            Update();
            var texts = FindObjectsOfType<Text>();
            foreach (var t in texts)
            {
                t.font = _font;
            }
        }

        private void Reset()
        {
            _selectable = ColorBlock.defaultColorBlock;
        }

        private void Update()
        {
            var selectables = Selectable.allSelectables;
            foreach (var s in selectables)
            {
                s.colors = _selectable;
            }
        }
    }
}
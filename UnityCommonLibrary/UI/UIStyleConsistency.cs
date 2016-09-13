using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary.UI
{
    [ExecuteInEditMode]
    public class UIStyleConsistency : MonoBehaviour
    {
        [SerializeField, Header("Selectables")]
        private ColorBlock selectable;
        [SerializeField, Header("Font")]
        private Font font;

        private void Update()
        {
            var selectables = Selectable.allSelectables;
            foreach (var s in selectables)
            {
                s.colors = selectable;
            }
        }

        public void FullUpdate()
        {
            Update();
            var texts = FindObjectsOfType<Text>();
            foreach (var t in texts)
            {
                t.font = font;
            }
        }

        private void Reset()
        {
            selectable = ColorBlock.defaultColorBlock;
        }
    }
}
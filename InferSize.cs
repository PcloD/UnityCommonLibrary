using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    [RequireComponent(typeof(LayoutElement))]
    public class InferSize : UCScript {
        public Graphic targetGraphic;
        public Vector2 minSizeOffset, preferredSizeOffset, flexibleSizeOffset;

        LayoutElement layout;

        void Awake() {
            layout = GetComponent<LayoutElement>();
            if(GetComponent<Button>() != null) {
                targetGraphic = GetComponentInChildren<Text>();
                preferredSizeOffset.x = 30;
            }
        }

        void Update() {
            if(targetGraphic != null) {
                layout.minWidth = LayoutUtility.GetMinWidth(targetGraphic.rectTransform) + minSizeOffset.x;
                layout.minHeight = LayoutUtility.GetMinHeight(targetGraphic.rectTransform) + minSizeOffset.y;
                layout.preferredWidth = LayoutUtility.GetPreferredWidth(targetGraphic.rectTransform) + preferredSizeOffset.x;
                layout.preferredHeight = LayoutUtility.GetPreferredHeight(targetGraphic.rectTransform) + preferredSizeOffset.y;
                layout.flexibleWidth = LayoutUtility.GetFlexibleWidth(targetGraphic.rectTransform) + flexibleSizeOffset.x;
                layout.flexibleHeight = LayoutUtility.GetFlexibleHeight(targetGraphic.rectTransform) + flexibleSizeOffset.y;
            }
        }
    }
}
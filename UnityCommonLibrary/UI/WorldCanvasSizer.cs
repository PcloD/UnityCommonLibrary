using UnityEngine;

namespace UnityCommonLibrary.UI
{
    [ExecuteInEditMode]
    public class WorldCanvasSizer : MonoBehaviour {
        public Vector2 resolution;
        public float meters;

        private RectTransform rect;

        private void Awake() {
            rect = GetComponent<RectTransform>();
            RefreshCanvasSize();
        }

        private void Reset() {
            resolution = new Vector2(1920f, 1080f);
            meters = 3f;
            RefreshCanvasSize();
        }

        private void OnValidate() {
            RefreshCanvasSize();
        }

        public void RefreshCanvasSize() {
            if(rect != null) {
                rect.sizeDelta = resolution;
                var scale = meters / resolution.x;
                rect.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }
}
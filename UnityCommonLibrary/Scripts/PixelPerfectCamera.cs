using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PixelPerfectCamera : MonoBehaviour {
        public bool useCurrentRes;
        public int screenWidth;
        public int screenHeight;
        public float pixelsPerUnit;

        private new Camera camera;

        private void Awake() {
            camera = GetComponent<Camera>();
        }

        private void Update() {
            float screenWidth = useCurrentRes ? Screen.currentResolution.width : this.screenWidth;
            float screenHeight = useCurrentRes ? Screen.currentResolution.height : this.screenHeight;
            camera.orthographicSize = screenWidth / (((screenWidth / screenHeight) * 2f) * pixelsPerUnit);
        }
    }
}

using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PixelPerfectOrthoSize : MonoBehaviour
    {
        public int designWidth;
        public int designHeight;
        public bool useWidth;
        public float pixelsPerUnit;

        private new Camera camera;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (!useWidth && designHeight != 0f)
            {
                camera.orthographicSize = (designHeight / pixelsPerUnit) / 2f;
            }
            else if (useWidth && designWidth != 0f)
            {
                camera.orthographicSize = (designWidth / pixelsPerUnit) / 2f;
            }
        }
    }
}

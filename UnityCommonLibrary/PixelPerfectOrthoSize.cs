using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PixelPerfectOrthoSize : MonoBehaviour
    {
        public int DesignHeight;
        public int DesignWidth;
        public float PixelsPerUnit;
        public bool UseWidth;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (!UseWidth && DesignHeight != 0f)
            {
                _camera.orthographicSize = DesignHeight / PixelsPerUnit / 2f;
            }
            else if (UseWidth && DesignWidth != 0f)
            {
                _camera.orthographicSize = DesignWidth / PixelsPerUnit / 2f;
            }
        }
    }
}
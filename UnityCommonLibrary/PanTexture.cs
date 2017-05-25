using UnityCommonLibrary.Time;
using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
    public class PanTexture : MonoBehaviour
    {
        public Vector2 Speed;
        public TimeMode TimeMode;
        private Material _material;
        private Vector2 _offset;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
        }

        private void Update()
        {
            _offset += Speed * TimeUtility.GetCurrentTime(TimeMode);
            _offset.x %= 1f;
            _offset.y %= 1f;
            _material.SetTextureOffset("_MainTex", _offset);
        }
    }
}
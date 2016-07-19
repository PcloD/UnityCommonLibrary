using UnityCommonLibrary.Utilities;
using UnityEngine;

namespace UnityCommonLibrary
{
    public class PanTexture : MonoBehaviour
    {
        public Vector2 speed;
        public Time.TimeMode timeMode;


        private new Renderer renderer;
        private Material material;
        private Vector2 offset;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            material = renderer.material;
        }

        private void Update()
        {
            offset += speed * TimeUtility.GetCurrentTime(timeMode);
            offset.x %= 1f;
            offset.y %= 1f;
            material.SetTextureOffset("_MainTex", offset);
        }

    }
}
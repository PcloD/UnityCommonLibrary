using UnityEngine;

namespace UnityCommonLibrary {
    public class PanTexture : MonoBehaviour {
        [SerializeField]
        public Vector2 speed;

        private new Renderer renderer;
        private Material material;
        private Vector2 offset;

        private void Awake() {
            renderer = GetComponent<Renderer>();
            material = renderer.material;
        }

        private void Update() {
            offset += speed * UnityEngine.Time.deltaTime;
            offset.x %= 1f;
            offset.y %= 1f;
            material.SetTextureOffset("_MainTex", offset);
        }

    }
}
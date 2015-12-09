using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(SpriteRenderer))]
    public class ColliderStore : MonoBehaviour {

        [SerializeField]
        StoreType type = StoreType.Circle;

        public enum StoreType { Box, Circle, Polygon }

        new SpriteRenderer renderer;
        Dictionary<Sprite, Collider2D> dictionary = new Dictionary<Sprite, Collider2D>();
        Sprite lastSprite;

        void Awake() {
            renderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update() {
            var s = renderer.sprite;
            if(s == lastSprite) {
                return;
            }

            Collider2D c2d;
            if(dictionary.TryGetValue(s, out c2d)) {
                if(c2d.enabled) {
                    return;
                }
            }
            else {
                switch(type) {
                    case StoreType.Box:
                        c2d = gameObject.AddComponent<BoxCollider2D>();
                        break;

                    case StoreType.Circle:
                        c2d = gameObject.AddComponent<CircleCollider2D>();
                        break;

                    case StoreType.Polygon:
                        c2d = gameObject.AddComponent<PolygonCollider2D>();
                        break;
                }

                c2d.hideFlags = HideFlags.HideInInspector;
                dictionary[s] = c2d;
            }

            foreach(var v in dictionary.Values) {
                v.enabled = false;
            }
            c2d.enabled = true;
            lastSprite = s;
        }
    }
}
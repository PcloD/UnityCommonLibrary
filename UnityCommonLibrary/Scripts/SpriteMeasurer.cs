using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteMeasurer : UCScript {
        PolygonCollider2D _p2d;
        public PolygonCollider2D p2d {
            get {
                if(_p2d == null) {
                    _p2d = GetComponent<PolygonCollider2D>();
                }
                return _p2d;
            }
        }

        SpriteRenderer _renderer;
        new public SpriteRenderer renderer {
            get {
                if(_renderer == null) {
                    _renderer = GetComponent<SpriteRenderer>();
                }
                return _renderer;
            }
        }
    }
}
using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public class CameraBounds2D : UCScript {
        [SerializeField]
        new Camera camera;
        [SerializeField]
        SpriteRenderer[] targetSprites;
        [SerializeField]
        Bounds bounds;
        [SerializeField]
        Color gizmosColor = Color.green;
        [SerializeField]
        bool boundedX = true, boundedY = true;

        Bounds camBounds;

        public bool canFit { get; private set; }

        void Awake() {
            if(camera == null) {
                camera = GetComponent<Camera>();
            }
        }

        void Update() {
            if(camera == null) {
                return;
            }

            camBounds = OrthoBounds();
            if(targetSprites != null) {
                bounds = new Bounds();
                foreach(var s in targetSprites) {
                    bounds.Encapsulate(s.bounds);
                }
            }

            canFit = bounds.size.y >= camBounds.size.y || !boundedY;
            canFit &= bounds.size.x >= camBounds.size.x || !boundedX;

            if(canFit) {
                ClampCamera();
            }
        }

        void ClampCamera() {
            var blFixed = bounds.min;
            var trFixed = bounds.max;
            blFixed.x += camBounds.size.x / 2f;
            trFixed.x -= camBounds.size.x / 2f;
            blFixed.y += camBounds.size.y / 2f;
            trFixed.y -= camBounds.size.y / 2f;

            var pos = transform.position;
            var newX = boundedX ? Mathf.Clamp(pos.x, blFixed.x, trFixed.x) : pos.x;
            var newY = boundedY ? Mathf.Clamp(pos.y, blFixed.y, trFixed.y) : pos.y;
            pos = new Vector3(newX, newY, pos.z);
            transform.position = pos;
        }

        /// <summary>
        /// Determine bounds of orthographic camera frustrum.
        /// </summary>
        /// <returns></returns>
        Bounds OrthoBounds() {
            var height = camera.orthographicSize * 2f;
            var bounds = new Bounds(camera.transform.position, new Vector3(height * camera.aspect, height, 0f));
            return bounds;
        }

        void OnDrawGizmosSelected() {
            var oldColor = Gizmos.color;
            Gizmos.color = gizmosColor;

            var bl = bounds.min;
            var tr = bounds.max;
            var tl = new Vector2(bl.x, tr.y);
            var br = new Vector2(tr.x, bl.y);
            Gizmos.DrawLine(bl, tl);
            Gizmos.DrawLine(tl, tr);
            Gizmos.DrawLine(tr, br);
            Gizmos.DrawLine(br, bl);

            Gizmos.color = oldColor;
        }
    }
}
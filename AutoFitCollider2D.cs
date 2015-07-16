using System;
using UnityEngine;
using UnityEngine.Sprites;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Collider2D))]
    public class AutoFitCollider2D : UCScript {
        [SerializeField]
        new SpriteRenderer renderer;
        [SerializeField]
        Collider2D c2d;

        Vector2 offset, size;

        Sprite sCurrent;

        void Awake() {
            if(renderer == null) {
                renderer = GetComponent<SpriteRenderer>();
            }
            sCurrent = renderer.sprite;
            CalculateOffsetAndSize();
        }

        void CalculateOffsetAndSize() {
            var padding = DataUtility.GetPadding(sCurrent);

            //Calculate size
            var width = sCurrent.bounds.size.x - ((padding.x + padding.z) / sCurrent.pixelsPerUnit);
            var height = sCurrent.bounds.size.y - ((padding.w + padding.y) / sCurrent.pixelsPerUnit);
            size = new Vector2(width, height);

            //Calculate offset
            offset = Vector2.zero;
            offset.x += padding.x / sCurrent.pixelsPerUnit;
            offset.x -= padding.z / sCurrent.pixelsPerUnit;
            offset.y -= padding.w / sCurrent.pixelsPerUnit;
            offset.y += padding.y / sCurrent.pixelsPerUnit;
            offset /= 2f;
        }

        void Update() {
            if(renderer.sprite != sCurrent) {
                sCurrent = renderer.sprite;
                CalculateOffsetAndSize();
            }

            if(c2d is BoxCollider2D) {
                UpdateBoxCollider();
            }
            else if(c2d is CircleCollider2D) {
                UpdateCircleCollider();
            }
        }

        private void UpdateCircleCollider() {
            var cc2d = c2d as CircleCollider2D;
            cc2d.radius = Mathf.Max(size.x, size.y) / 2f;
            cc2d.offset = offset;
        }

        private void UpdateBoxCollider() {
            var bc2d = c2d as BoxCollider2D;
            bc2d.size = size;
            bc2d.offset = offset;
        }
    }
}
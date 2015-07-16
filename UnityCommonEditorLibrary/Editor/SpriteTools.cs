using UnityEngine;
using UnityEditor;
using UnityCommonLibrary;

namespace UnityCommonEditorLibrary {
    public class SpriteTools : UCScript {

        [MenuItem("CONTEXT/Rigidbody2D/Ground Sprite")]
        public static void GroundSprite(MenuCommand command) {
            var rb2d = command.context as Rigidbody2D;
            var p2d = rb2d.gameObject.AddComponent<PolygonCollider2D>();
            var hits2D = Physics2D.RaycastAll(rb2d.transform.position, Vector2.down, float.MaxValue);
            foreach(var h2d in hits2D) {
                if(h2d.collider.gameObject != rb2d.gameObject) {
                    var diff = h2d.distance - p2d.bounds.extents.y;
                    diff *= -1f;
                    rb2d.transform.position += Vector3.up * diff;
                }
            }

            DestroyImmediate(p2d);
        }

    }
}
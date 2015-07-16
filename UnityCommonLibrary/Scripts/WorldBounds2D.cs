using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Collider2D))]
    public class WorldBounds2D : WorldBounds {

        public Collider2D c2d { get; private set; }

        public void OnTriggerEnter2D(Collider2D collision) {
            SendAlertEntered(collision.gameObject, "OnEnteredWorldBounds2D");
        }

        public void OnTriggerExit2D(Collider2D collision) {
            SendAlertExited(collision.gameObject, "OnLeftWorldBounds2D");
        }

        protected override void EnforceBehaviors() {
            if(c2d == null) {
                c2d = GetComponent<Collider2D>();
            }
            c2d.sharedMaterial = null;
            c2d.usedByEffector = false;
            c2d.isTrigger = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }
    }
}
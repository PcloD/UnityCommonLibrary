using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Collider2D))]
    public class WorldBounds2D : WorldBounds {

        public Collider2D c2d { get; private set; }

        public void OnTriggerEnter2D(Collider2D collider) {
            SendAlertEntered(collider.gameObject);
        }

        public void OnTriggerExit2D(Collider2D collider) {
            SendAlertExited(collider.gameObject);
        }
    }
}
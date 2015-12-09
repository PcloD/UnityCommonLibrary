using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Collider))]
    public class WorldBounds3D : WorldBounds {

        public Collider c { get; private set; }

        public void OnTriggerEnter(Collider other) {
            SendAlertEntered(other.gameObject);
        }

        public void OnTriggerExit(Collider other) {
            SendAlertExited(other.gameObject);
        }
    }
}
using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Collider))]
    public class WorldBounds3D : WorldBounds {

        public Collider c { get; private set; }

        public void OnTriggerEnter(Collider other) {
            SendAlertEntered(other.gameObject, "OnEnteredWorldBounds");
        }

        public void OnTriggerExit(Collider other) {
            SendAlertExited(other.gameObject, "OnLeftWorldBounds");
        }

        protected override void EnforceBehaviors() {
            if(c == null) {
                c = GetComponent<Collider>();
            }
            c.isTrigger = true;
            c.sharedMaterial = null;
        }
    }
}
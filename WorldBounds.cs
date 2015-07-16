using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public abstract class WorldBounds : UCScript {

        void Update() {
            transform.localScale = Vector3.one;
            EnforceBehaviors();
        }

        protected void SendAlertEntered(GameObject g, string message) {
            g.SendMessageUpwards(message, this, SendMessageOptions.DontRequireReceiver);
        }

        protected void SendAlertExited(GameObject g, string message) {
            g.SendMessageUpwards(message, this, SendMessageOptions.DontRequireReceiver);
        }

        protected abstract void EnforceBehaviors();
    }
}
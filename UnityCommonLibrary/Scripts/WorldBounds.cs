using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public abstract class WorldBounds : MonoBehaviour {
        protected void SendAlertEntered(GameObject obj) {
            var listeners = obj.GetComponents<WorldBoundsEventListener>();
            foreach(var l in listeners) {
                l.OnEnteredWorldBounds();
            }
        }

        protected void SendAlertExited(GameObject obj) {
            var listeners = obj.GetComponents<WorldBoundsEventListener>();
            foreach(var l in listeners) {
                l.OnLeftWorldBounds();
            }
        }
    }

    public interface WorldBoundsEventListener {
        void OnEnteredWorldBounds();
        void OnLeftWorldBounds();
    }
}
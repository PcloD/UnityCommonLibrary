using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public abstract class WorldBounds : MonoBehaviour {
        protected void SendAlertEntered(GameObject obj) {
            var listeners = obj.GetComponents<IWorldBoundsEventListener>();
            foreach(var l in listeners) {
                l.OnEnteredWorldBounds();
            }
        }

        protected void SendAlertExited(GameObject obj) {
            var listeners = obj.GetComponents<IWorldBoundsEventListener>();
            foreach(var l in listeners) {
                l.OnLeftWorldBounds();
            }
        }
    }

    public interface IWorldBoundsEventListener {
        void OnEnteredWorldBounds();
        void OnLeftWorldBounds();
    }
}
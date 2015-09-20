using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public class LogicalObject : UCScript {

        void Update() {
            transform.hideFlags = HideFlags.HideInInspector;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        void OnDestroy() {
            transform.hideFlags = HideFlags.None;
        }

    }
}
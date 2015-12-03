using UnityCommonLibrary;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    public class Ruler : UCScript {
        [SerializeField]
        protected internal Vector3 end;
        [SerializeField]
        protected internal Transform endTransform;

        [SerializeField]
        [ReadOnly]
        internal float distance;

        internal Vector3 selectedEnd {
            get {
                return endTransform == null ? end : endTransform.position;
            }
        }

        public void OnDrawGizmos() {
            if(enabled) {
                Gizmos.DrawLine(transform.position, selectedEnd);
                distance = Vector3.Distance(transform.position, selectedEnd);
            }
        }

        void Update() {
            enabled = false;
        }

        void OnValidate() {
            hideFlags = HideFlags.DontSaveInBuild;
        }
    }
}
using UnityEngine;

namespace UnityCommonLibrary {
    public class FloatRange : MonoBehaviour {
        [SerializeField]
        Vector3 offset;

        public float min, max;

        public void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position + offset, min);
            Gizmos.DrawWireSphere(transform.position + offset, max);
        }

        void Update() {
            OnValidate();
        }

        void OnValidate() {
            if(min > max) {
                min = max;
            }
        }

        public bool CheckInclusive(float f) {
            return f >= min && f <= max;
        }

        public bool CheckExclusive(float f) {
            return f > min && f < max;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommonLibrary.Colliders {
    [RequireComponent(typeof(Collider))]
    public class ColliderUnityEvents : MonoBehaviour {
        [Serializable]
        public class OnCollisionEvent : UnityEvent<ColliderUnityEvents, Collision> { }
        [Serializable]
        public class OnTriggerEvent : UnityEvent<ColliderUnityEvents, Collider> { }

        public OnCollisionEvent collisionEnter;
        public OnCollisionEvent collisionExit;
        public OnCollisionEvent collisionStay;

        public OnTriggerEvent triggerEnter;
        public OnTriggerEvent triggerExit;
        public OnTriggerEvent triggerStay;

        public Collider eventCollider { get; private set; }

        private void Awake() {
            eventCollider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision) {
            collisionEnter.Invoke(this, collision);
        }

        private void OnCollisionExit(Collision collision) {
            collisionExit.Invoke(this, collision);
        }

        private void OnCollisionStay(Collision collision) {
            collisionStay.Invoke(this, collision);
        }

        private void OnTriggerEnter(Collider other) {
            triggerEnter.Invoke(this, other);
        }

        private void OnTriggerExit(Collider other) {
            triggerExit.Invoke(this, other);
        }

        private void OnTriggerStay(Collider other) {
            triggerStay.Invoke(this, other);
        }
    }
}
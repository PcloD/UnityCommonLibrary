using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Collider))]
    public class ColliderUnityEvents : MonoBehaviour {
        [Serializable]
        public class OnCollisionEvent : UnityEvent<ColliderUnityEvents, Collision> { }
        [Serializable]
        public class OnTriggerEvent : UnityEvent<ColliderUnityEvents, Collider> { }

        [Header("Collision Events")]
        [DisplayName("On Enter")]
        public OnCollisionEvent collisionEnter;
        [DisplayName("On Exit")]
        public OnCollisionEvent collisionExit;
        [DisplayName("On Stay")]
        public OnCollisionEvent collisionStay;

        [Header("Trigger Events")]
        [DisplayName("On Enter")]
        public OnTriggerEvent triggerEnter;
        [DisplayName("On Exit")]
        public OnTriggerEvent triggerExit;
        [DisplayName("On Stay")]
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
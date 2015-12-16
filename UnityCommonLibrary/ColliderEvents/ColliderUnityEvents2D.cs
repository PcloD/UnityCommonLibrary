using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Collider2D))]
    public class ColliderUnityEvents2D : MonoBehaviour {
        [Serializable]
        public class OnCollisionEvent2D : UnityEvent<ColliderUnityEvents2D, Collision2D> { }
        [Serializable]
        public class OnTriggerEvent2D : UnityEvent<ColliderUnityEvents2D, Collider2D> { }

        [Header("Collision Events")]
        [DisplayName("On Enter 2D")]
        public OnCollisionEvent2D collisionEnter2D;
        [DisplayName("On Exit 2D")]
        public OnCollisionEvent2D collisionExit2D;
        [DisplayName("On Stay 2D")]
        public OnCollisionEvent2D collisionStay2D;

        [Header("Trigger Events")]
        [DisplayName("On Enter 2D")]
        public OnTriggerEvent2D triggerEnter2D;
        [DisplayName("On Exit 2D")]
        public OnTriggerEvent2D triggerExit2D;
        [DisplayName("On Stay 2D")]
        public OnTriggerEvent2D triggerStay2D;

        public Collider2D eventCollider2D { get; private set; }

        private void Awake() {
            eventCollider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            collisionEnter2D.Invoke(this, collision);
        }

        private void OnCollisionExit2D(Collision2D collision) {
            collisionExit2D.Invoke(this, collision);
        }

        private void OnCollisionStay2D(Collision2D collision) {
            collisionStay2D.Invoke(this, collision);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            triggerEnter2D.Invoke(this, other);
        }

        private void OnTriggerExit2D(Collider2D other) {
            triggerExit2D.Invoke(this, other);
        }

        private void OnTriggerStay2D(Collider2D other) {
            triggerStay2D.Invoke(this, other);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommonLibrary.Colliders
{
    /// <summary>
    ///     A behaviour for responding to collision and trigger events
    ///     outside of the GameObject that stores the Collider2D component.
    ///     This flavor uses <see cref="UnityEvent{T0,T1}" /> to hook up
    ///     callbacks to events in the editor.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class ColliderUnityEvents2D : MonoBehaviour
    {
        [Serializable]
        public class
            OnCollisionEvent2D : UnityEvent<ColliderUnityEvents2D, Collision2D> { }

        [Serializable]
        public class OnTriggerEvent2D : UnityEvent<ColliderUnityEvents2D, Collider2D> { }

        public OnCollisionEvent2D CollisionEnter2D;
        public OnCollisionEvent2D CollisionExit2D;
        public OnCollisionEvent2D CollisionStay2D;
        public OnTriggerEvent2D TriggerEnter2D;
        public OnTriggerEvent2D TriggerExit2D;
        public OnTriggerEvent2D TriggerStay2D;

        public Collider2D EventCollider2D { get; private set; }

        private void Awake()
        {
            EventCollider2D = GetComponent<Collider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEnter2D.Invoke(this, collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            CollisionExit2D.Invoke(this, collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            CollisionStay2D.Invoke(this, collision);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            TriggerEnter2D.Invoke(this, other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            TriggerExit2D.Invoke(this, other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            TriggerStay2D.Invoke(this, other);
        }
    }
}
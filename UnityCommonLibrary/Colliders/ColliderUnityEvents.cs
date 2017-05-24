using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommonLibrary.Colliders
{
    /// <summary>
    ///     A behaviour for responding to collision and trigger events
    ///     outside of the GameObject that stores the Collider component.
    ///     This flavor uses <see cref="UnityEvent{T0,T1}" /> to hook up
    ///     callbacks to events in the editor.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ColliderUnityEvents : MonoBehaviour
    {
        [Serializable]
        public class OnCollisionEvent : UnityEvent<ColliderUnityEvents, Collision> { }

        [Serializable]
        public class OnTriggerEvent : UnityEvent<ColliderUnityEvents, Collider> { }

        public OnCollisionEvent CollisionEnter;
        public OnCollisionEvent CollisionExit;
        public OnCollisionEvent CollisionStay;
        public OnTriggerEvent TriggerEnter;
        public OnTriggerEvent TriggerExit;
        public OnTriggerEvent TriggerStay;

        public Collider EventCollider { get; private set; }

        private void Awake()
        {
            EventCollider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnter.Invoke(this, collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            CollisionExit.Invoke(this, collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            CollisionStay.Invoke(this, collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter.Invoke(this, other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExit.Invoke(this, other);
        }

        private void OnTriggerStay(Collider other)
        {
            TriggerStay.Invoke(this, other);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommonLibrary.Colliders
{
	/// <summary>
	/// A behaviour for responding to collision and trigger events
	/// outside of the GameObject that stores the Collider component.
	/// This flavor uses <see cref="UnityEvent{T0,T1}"/> to hook up
	/// callbacks to events in the editor.
	/// </summary>
	[RequireComponent(typeof(Collider))]
	public class ColliderUnityEvents : MonoBehaviour
	{
		#region UnityEvents
		public OnCollisionEvent collisionEnter;
		public OnCollisionEvent collisionExit;
		public OnCollisionEvent collisionStay;
		public OnTriggerEvent triggerEnter;
		public OnTriggerEvent triggerExit;
		public OnTriggerEvent triggerStay;
		#endregion

		public Collider eventCollider { get; private set; }

		#region Unity Messages
		private void Awake()
		{
			eventCollider = GetComponent<Collider>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			collisionEnter.Invoke(this, collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			collisionExit.Invoke(this, collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			collisionStay.Invoke(this, collision);
		}

		private void OnTriggerEnter(Collider other)
		{
			triggerEnter.Invoke(this, other);
		}

		private void OnTriggerExit(Collider other)
		{
			triggerExit.Invoke(this, other);
		}

		private void OnTriggerStay(Collider other)
		{
			triggerStay.Invoke(this, other);
		}
		#endregion

		#region Classes
		[Serializable]
		public class OnCollisionEvent : UnityEvent<ColliderUnityEvents, Collision> { }
		[Serializable]
		public class OnTriggerEvent : UnityEvent<ColliderUnityEvents, Collider> { }
		#endregion
	}
}
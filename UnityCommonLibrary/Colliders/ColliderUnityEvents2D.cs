using System;
using UnityEngine;
using UnityEngine.Events;

namespace UnityCommonLibrary.Colliders {
	/// <summary>
	/// A behaviour for responding to collision and trigger events
	/// outside of the GameObject that stores the Collider2D component.
	/// This flavor uses <see cref="UnityEvent{T0,T1}"/> to hook up
	/// callbacks to events in the editor.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class ColliderUnityEvents2D : MonoBehaviour {
		#region UnityEvents
		public OnCollisionEvent2D collisionEnter2D;
		public OnCollisionEvent2D collisionExit2D;
		public OnCollisionEvent2D collisionStay2D;
		public OnTriggerEvent2D triggerEnter2D;
		public OnTriggerEvent2D triggerExit2D;
		public OnTriggerEvent2D triggerStay2D;
		#endregion

		public Collider2D eventCollider2D { get; private set; }

		#region Unity Messages
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
		#endregion

		#region Classes
		[Serializable]
		public class OnCollisionEvent2D : UnityEvent<ColliderUnityEvents2D, Collision2D> { }
		[Serializable]
		public class OnTriggerEvent2D : UnityEvent<ColliderUnityEvents2D, Collider2D> { }
		#endregion
	}
}
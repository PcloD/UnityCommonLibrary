using UnityEngine;

namespace UnityCommonLibrary.Colliders {
	/// <summary>
	/// A behaviour for responding to collision and trigger events
	/// outside of the GameObject that stores the Collider2D component.
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class ColliderEvents2D : MonoBehaviour {
		#region Delegates/Events
		public delegate void OnCollisionEvent2D(ColliderEvents2D source, Collision2D collision);
		public delegate void OnTriggerEvent2D(ColliderEvents2D source, Collider2D other);

		public event OnCollisionEvent2D CollisionEnter2D;
		public event OnCollisionEvent2D CollisionExit2D;
		public event OnCollisionEvent2D CollisionStay2D;
		public event OnTriggerEvent2D TriggerEnter2D;
		public event OnTriggerEvent2D TriggerExit2D;
		public event OnTriggerEvent2D TriggerStay2D;
		#endregion

		public Collider2D eventCollider { get; private set; }

		#region Unity Messages
		private void Awake() {
			eventCollider = GetComponent<Collider2D>();
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			if(CollisionEnter2D != null) {
				CollisionEnter2D(this, collision);
			}
		}

		private void OnCollisionExit2D(Collision2D collision) {
			if(CollisionExit2D != null) {
				CollisionExit2D(this, collision);
			}
		}

		private void OnCollisionStay2D(Collision2D collision) {
			if(CollisionStay2D != null) {
				CollisionStay2D(this, collision);
			}
		}

		private void OnTriggerEnter2D(Collider2D other) {
			if(TriggerEnter2D != null) {
				TriggerEnter2D(this, other);
			}
		}

		private void OnTriggerExit2D(Collider2D other) {
			if(TriggerExit2D != null) {
				TriggerExit2D(this, other);
			}
		}

		private void OnTriggerStay2D(Collider2D other) {
			if(TriggerStay2D != null) {
				TriggerStay2D(this, other);
			}
		}
		#endregion
	}
}
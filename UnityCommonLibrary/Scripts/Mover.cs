using UnityEngine;

namespace UnityCommonLibrary
{
	public class Mover : MonoBehaviour
	{
		[SerializeField]
		private Vector3 movement = Vector3.one;
		[SerializeField]
		private Space movementSpace;
		[SerializeField]
		private Vector3 rotation = Vector3.one;
		[SerializeField]
		private Space rotationSpace;

		private new Rigidbody rigidbody;

		private void Update()
		{
			if (rigidbody)
			{
				if (movementSpace == Space.Self)
				{
					rigidbody.velocity = transform.TransformVector(movement);
					rigidbody.angularVelocity = transform.TransformVector(rotation);
				}
				else
				{
					rigidbody.velocity = movement;
					rigidbody.angularVelocity = rotation;
				}
			}
			else {
				transform.Translate(movement, movementSpace);
				transform.Rotate(rotation, rotationSpace);
			}
		}
	}
}
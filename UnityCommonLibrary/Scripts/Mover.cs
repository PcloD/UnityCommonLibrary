using UnityEngine;

namespace UnityCommonLibrary
{
    public class Mover : MonoBehaviour
    {
        public Vector3 movement = Vector3.one;
        public Space movementSpace;
        public Vector3 rotation = Vector3.one;
        public Space rotationSpace;
        public bool useRigidbody;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            if (useRigidbody && rigidbody != null)
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
            else
            {
                transform.Translate(movement, movementSpace);
                transform.Rotate(rotation, rotationSpace);
            }
        }
    }
}
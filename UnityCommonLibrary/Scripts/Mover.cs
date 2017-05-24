using UnityEngine;

namespace UnityCommonLibrary
{
    public class Mover : MonoBehaviour
    {
        public Vector3 Movement = Vector3.one;
        public Space MovementSpace;
        public Vector3 Rotation = Vector3.one;
        public Space RotationSpace;
        public bool UseRigidbody;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (UseRigidbody && _rigidbody != null)
            {
                if (MovementSpace == Space.Self)
                {
                    _rigidbody.velocity = transform.TransformVector(Movement);
                    _rigidbody.angularVelocity = transform.TransformVector(Rotation);
                }
                else
                {
                    _rigidbody.velocity = Movement;
                    _rigidbody.angularVelocity = Rotation;
                }
            }
            else
            {
                transform.Translate(Movement, MovementSpace);
                transform.Rotate(Rotation, RotationSpace);
            }
        }
    }
}
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

        private void Update()
        {
            transform.Translate(movement, movementSpace);
            transform.Rotate(rotation, rotationSpace);
        }

    }
}
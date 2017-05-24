using UnityEngine;

namespace UnityCommonLibrary
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravityModifier : MonoBehaviour
    {
        private ConstantForce _force;

        [SerializeField]
        private float _multiplier;

        private Rigidbody _rb;

        private void Awake()
        {
            _force = gameObject.AddComponent<ConstantForce>();
            _rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            _force.force = -Physics.gravity * _rb.mass * _multiplier;
        }
    }
}
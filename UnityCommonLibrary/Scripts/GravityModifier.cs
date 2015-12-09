using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Rigidbody))]
    public class GravityModifier : MonoBehaviour {

        [SerializeField]
        float multiplier = 0f;

        ConstantForce force;
        Rigidbody rb;

        void Awake() {
            force = gameObject.AddComponent<ConstantForce>();
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update() {
            force.force = (-Physics.gravity * rb.mass) * multiplier;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary {
    public class AreaEffector : UCScript {
        [SerializeField, Header("Force")]
        Vector3 force;
        [SerializeField]
        ForceMode forceMode = ForceMode.Force;
        [SerializeField]
        bool disableGravity, ignoreMass;
        [SerializeField, Header("Torque")]
        Vector3 torque;
        [SerializeField]
        ForceMode torqueMode = ForceMode.Force;

        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        List<Rigidbody> nullRigidbodies = new List<Rigidbody>();

        void OnTriggerEnter(Collider c) {

            if(c.attachedRigidbody != null) {
                if(disableGravity) {
                    c.attachedRigidbody.useGravity = false;
                }
                rigidbodies.Add(c.attachedRigidbody);
            }
        }

        void OnTriggerExit(Collider c) {
            if(c.attachedRigidbody != null) {
                if(disableGravity) {
                    c.attachedRigidbody.useGravity = true;
                }
                rigidbodies.Remove(c.attachedRigidbody);
            }
        }

        void FixedUpdate() {
            //Localized force we can change mid loop
            var _force = force;
            foreach(var rb in rigidbodies) {
                if(rb == null) {
                    nullRigidbodies.Add(rb);
                }
                else {
                    if(ignoreMass && forceMode == ForceMode.Force || forceMode == ForceMode.Impulse) {
                        _force *= rb.mass;
                    }
                    rb.AddForce(_force, forceMode);
                    rb.AddTorque(torque, torqueMode);
                }
            }
            foreach(var nullRB in nullRigidbodies) {
                rigidbodies.Remove(nullRB);
            }
            nullRigidbodies.Clear();
        }

    }
}
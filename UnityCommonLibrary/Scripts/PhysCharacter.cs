using UnityEngine;

namespace UnityCommonLibrary
{
    [RequireComponent(typeof(Animator))]
    public class PhysCharacter : MonoBehaviour {

        [SerializeField]
        Transform skeletonRoot;

        public Rigidbody[] ragdollBodies { get; private set; }

        public Collider[] ragdollColliders { get; private set; }

        public bool isRagdoll;

        Animator animator;
        bool wasRagdoll;

        void Awake() {
            ragdollBodies = GetComponentsInChildren<Rigidbody>();
            animator = GetComponent<Animator>();
            ragdollColliders = GetComponentsInChildren<Collider>();
            EnsureMode();
        }

        void LateUpdate() {
            if(isRagdoll != wasRagdoll) {
                EnsureMode();
            }
        }

        public Transform FindRagdollLimb(string name) {
            return skeletonRoot.FindChild(name);
        }

        void EnsureMode() {
            foreach(var r in ragdollBodies) {
                r.isKinematic = !isRagdoll;
            }
            foreach(var c in ragdollColliders) {
                c.isTrigger = !isRagdoll;
            }
            animator.enabled = !isRagdoll;
            wasRagdoll = isRagdoll;
        }

        public void Swap() {
            isRagdoll = !isRagdoll;
        }
    }
}
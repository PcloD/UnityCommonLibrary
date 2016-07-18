using UnityEngine;

namespace UnityCommonLibrary
{
    [RequireComponent(typeof(Animator))]
    public class PhysCharacter : MonoBehaviour
    {
        public bool isRagdoll;

        [SerializeField]
        private Transform skeletonRoot;

        public Rigidbody[] ragdollBodies { get; private set; }
        public Collider[] ragdollColliders { get; private set; }

        private Animator animator;
        private bool wasRagdoll;

        public Transform FindRagdollLimb(string name)
        {
            return skeletonRoot.FindChild(name);
        }
        public void Swap()
        {
            isRagdoll = !isRagdoll;
        }

        private void Awake()
        {
            ragdollBodies = GetComponentsInChildren<Rigidbody>();
            animator = GetComponent<Animator>();
            ragdollColliders = GetComponentsInChildren<Collider>();
            EnsureMode();
        }
        private void LateUpdate()
        {
            if (isRagdoll != wasRagdoll)
            {
                EnsureMode();
            }
        }
        private void EnsureMode()
        {
            foreach (var r in ragdollBodies)
            {
                r.isKinematic = !isRagdoll;
            }
            foreach (var c in ragdollColliders)
            {
                c.isTrigger = !isRagdoll;
            }
            animator.enabled = !isRagdoll;
            wasRagdoll = isRagdoll;
        }
    }
}
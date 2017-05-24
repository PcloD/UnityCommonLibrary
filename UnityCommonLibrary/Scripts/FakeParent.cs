using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    public class FakeParent : MonoBehaviour
    {
        public bool DestroyWithParent;
        public Vector3 LocalPosition;
        public Vector3 LocalRotation;
        public Transform OriginalParent;
        public bool UsePosition;
        public bool UseRotation;

        private bool _hadParent;

        private void Update()
        {
            if (!OriginalParent && _hadParent && DestroyWithParent)
            {
                Destroy(gameObject);
                return;
            }
            if (!DestroyWithParent)
            {
                return;
            }
            if (OriginalParent)
            {
                _hadParent = true;
            }

            if (UsePosition)
            {
                transform.position = OriginalParent.transform.position + LocalPosition;
            }
            if (UseRotation)
            {
                transform.rotation =
                    Quaternion.Euler(OriginalParent.transform.eulerAngles +
                                     LocalRotation);
            }
        }
    }
}
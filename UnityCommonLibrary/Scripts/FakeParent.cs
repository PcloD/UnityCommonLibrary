using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    public class FakeParent : MonoBehaviour
    {
        public Transform fakeParent;
        public bool usePosition;
        public Vector3 localPosition;
        public bool useRotation;
        public Vector3 localRotation;
        public bool destroyWithParent;

        private bool hadParent;

        private void Update()
        {
            if (!fakeParent && hadParent && destroyWithParent)
            {
                Destroy(gameObject);
                return;
            }
            else if (!destroyWithParent)
            {
                return;
            }
            else if (fakeParent)
            {
                hadParent = true;
            }

            if (usePosition)
            {
                transform.position = fakeParent.transform.position + localPosition;
            }
            if (useRotation)
            {
                transform.rotation = Quaternion.Euler(fakeParent.transform.eulerAngles + localRotation);
            }
        }

    }
}
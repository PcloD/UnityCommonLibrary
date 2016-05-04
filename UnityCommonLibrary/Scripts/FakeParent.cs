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

        private void Update()
        {
            if (!fakeParent)
            {
                return;
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
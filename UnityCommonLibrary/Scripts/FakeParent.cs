using UnityEngine;

namespace UnityCommonLibrary {
    public class FakeParent : MonoBehaviour {
        public Vector3 localPosition;
        public Transform fakeParent;

        private void Update() {
            transform.position = fakeParent.transform.position + localPosition;
        }

    }
}
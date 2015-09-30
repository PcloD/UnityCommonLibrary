using UnityEngine;

namespace UnityCommonLibrary {
    public class FakeParent : UCScript {
        public Transform fakeParent;

        void Update() {
            transform.position = fakeParent.transform.position + transform.position;
        }

    }
}
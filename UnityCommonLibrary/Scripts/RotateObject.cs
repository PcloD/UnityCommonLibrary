using UnityEngine;

namespace UnityCommonLibrary {
    public class RotateObject : MonoBehaviour {

        [SerializeField]
        Vector3 axis = Vector3.one;

        [SerializeField]
        float speed = 10f;

        void Update() {
            transform.Rotate(axis, speed * Time.unscaledDeltaTime, Space.Self);
        }
    }
}
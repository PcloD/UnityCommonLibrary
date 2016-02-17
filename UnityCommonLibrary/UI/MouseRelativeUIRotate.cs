using UnityEngine;

namespace UnityCommonLibrary.UI {
    public class MouseRelativeUIRotate : MonoBehaviour {

        [SerializeField]
        Vector2 followMultiplier;

        RectTransform rect;

        void Awake() {
            rect = GetComponent<RectTransform>();
        }

        void Update() {
            var m = Input.mousePosition;
            var center = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var dist = m - center;

            rect.localRotation = Quaternion.identity;
            rect.Rotate(Vector3.right, -dist.y * followMultiplier.y, Space.Self);
            rect.Rotate(Vector3.up, dist.x * followMultiplier.x, Space.Self);
        }
    }
}
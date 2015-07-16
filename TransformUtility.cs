using UnityEngine;

namespace UnityCommonLibrary {
    public static class TransformUtility {

        public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.position;
            SetV3Parts(ref newV3, x, y, z);
            transform.position = newV3;
        }

        public static void SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.localPosition;
            SetV3Parts(ref newV3, x, y, z);
            transform.localPosition = newV3;
        }

        public static void SetLocalScale(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.localScale;
            SetV3Parts(ref newV3, x, y, z);
            transform.localScale = newV3;
        }

        public static void SetEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.eulerAngles;
            SetV3Parts(ref newV3, x, y, z);
            transform.rotation = Quaternion.Euler(newV3);
        }

        public static void SetLocalEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.localEulerAngles;
            SetV3Parts(ref newV3, x, y, z);
            transform.localRotation = Quaternion.Euler(newV3);
        }

        static void SetV3Parts(ref Vector3 v3, float? x = null, float? y = null, float? z = null) {
            if(x.HasValue) {
                v3.x = x.Value;
            }
            if(y.HasValue) {
                v3.y = y.Value;
            }
            if(z.HasValue) {
                v3.z = z.Value;
            }
        }

    }
}

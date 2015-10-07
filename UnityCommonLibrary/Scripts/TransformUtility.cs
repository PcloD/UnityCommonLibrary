using UnityEngine;

namespace UnityCommonLibrary {
    public static class TransformUtility {

        public static void SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.position;
            newV3.SetXYZ(x, y, z);
            transform.position = newV3;
        }

        public static void SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.localPosition;
            newV3.SetXYZ(x, y, z);
            transform.localPosition = newV3;
        }

        public static void SetLocalScale(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.localScale;
            newV3.SetXYZ(x, y, z);
            transform.localScale = newV3;
        }

        public static void SetEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.eulerAngles;
            newV3.SetXYZ(x, y, z);
            transform.rotation = Quaternion.Euler(newV3);
        }

        public static void SetLocalEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.localEulerAngles;
            newV3.SetXYZ(x, y, z);
            transform.localRotation = Quaternion.Euler(newV3);
        }

        public static void SetXYZ(this Vector3 v3, float? x = null, float? y = null, float? z = null) {
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

        public static void SetWXYZ(this Quaternion q, float? w = null, float? x = null, float? y = null, float? z = null) {
            if(w.HasValue) {
                q.w = w.Value;
            }
            if(x.HasValue) {
                q.x = x.Value;
            }
            if(y.HasValue) {
                q.y = y.Value;
            }
            if(z.HasValue) {
                q.z = z.Value;
            }
        }

    }
}
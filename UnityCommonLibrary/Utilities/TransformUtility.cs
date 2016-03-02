using UnityEngine;

namespace UnityCommonLibrary.Utilities {
    public static class TransformUtility {

        public static void SetPosition(this Transform transform, Space space, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.position;
            newV3.SetXYZ(x, y, z);
            if(space == Space.World) {
                transform.position = newV3;
            }
            else {
                transform.localPosition = newV3;
            }
        }

        public static void SetLocalScale(this Transform transform, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.localScale;
            newV3.SetXYZ(x, y, z);
            transform.localScale = newV3;
        }

        public static void SetEulerAngles(this Transform transform, Space space, float? x = null, float? y = null, float? z = null) {
            var newV3 = transform.eulerAngles;
            newV3.SetXYZ(x, y, z);
            if(space == Space.World) {
                transform.rotation = Quaternion.Euler(newV3);
            }
            else {
                transform.localRotation = Quaternion.Euler(newV3);
            }
        }

        private static void SetXYZ(this Vector3 v3, float? x = null, float? y = null, float? z = null) {
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

        public static void Reset(this Transform t) {
            Reset(t, TransformElement.All, Space.World);
        }

        public static void Reset(this Transform t, TransformElement elements) {
            Reset(t, elements, Space.World);
        }

        public static void Reset(this Transform t, Space space) {
            Reset(t, TransformElement.All, space);
        }

        public static void Reset(this Transform t, TransformElement elements, Space space) {
            if((elements & TransformElement.Position) != 0) {
                if(space == Space.World) {
                    t.position = Vector3.zero;
                }
                else {
                    t.localPosition = Vector3.zero;
                }
            }
            if((elements & TransformElement.Rotation) != 0) {
                if(space == Space.World) {
                    t.rotation = Quaternion.identity;
                }
                else {
                    t.localRotation = Quaternion.identity;
                }
            }
            if((elements & TransformElement.Scale) != 0) {
                t.localScale = Vector3.one;
            }
        }
    }
}
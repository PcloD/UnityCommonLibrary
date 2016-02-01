using UnityEngine;

namespace UnityCommonLibrary {
    public class VectorUtility {

        private static Vector3 RoundTo(Vector3 vec, float nearest) {
            if(Mathf.Approximately(nearest, 0f)) {
                return vec;
            }
            return new Vector3() {
                x = MathUtility.RoundTo(vec.x, nearest),
                y = MathUtility.RoundTo(vec.y, nearest),
                z = MathUtility.RoundTo(vec.z, nearest)
            };
        }

        private static Vector3 RoundTo(Vector2 vec, float nearest) {
            if(Mathf.Approximately(nearest, 0f)) {
                return vec;
            }
            return new Vector2() {
                x = MathUtility.RoundTo(vec.x, nearest),
                y = MathUtility.RoundTo(vec.y, nearest)
            };
        }

    }
}

using UnityEngine;

namespace UnityCommonLibrary.Utilities
{
    public static class DebugUtility
    {
        public static string GetDebugName(this Component cmp)
        {
            return GetDebugName(cmp.gameObject);
        }
        public static string GetDebugName(this GameObject go)
        {
            return string.Format("[{0}] {1}", go.GetInstanceID(), go.name);
        }
        public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            DrawArrow(pos, direction, Color.white, arrowHeadLength, arrowHeadAngle);
        }
        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Debug.DrawRay(pos, direction, color);
            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
            Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
        }
    }
}
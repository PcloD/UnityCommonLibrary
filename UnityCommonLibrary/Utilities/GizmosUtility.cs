using UnityEngine;

namespace UnityCommonLibrary.Utilities
{
    public static class GizmosUtility
    {
        private static Color storedColor;

        public static void DrawBounds(Bounds b)
        {
            Gizmos.DrawWireCube(b.center, b.size);
        }
        /// <summary>
        /// Stores the current Gizmos color and switches to a new color.
        /// Used in conjunction with <see cref="RestoreColor"/>
        /// </summary>
        /// <param name="color"></param>
        public static void StoreColor(Color color)
        {
            storedColor = Gizmos.color;
            Gizmos.color = color;
        }
        /// <summary>
        /// Restores the stored Gizmos color from <see cref="StoreColor(Color)"/>
        /// </summary>
        public static void RestoreColor()
        {
            Gizmos.color = storedColor;
        }
        public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            DrawArrow(pos, direction, Gizmos.color, arrowHeadLength, arrowHeadAngle);
        }
        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            Gizmos.color = color;
            Gizmos.DrawRay(pos, direction);

            var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}
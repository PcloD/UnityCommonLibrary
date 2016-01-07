using UnityEngine;

namespace UnityCommonLibrary {
    public static class GizmosUtility {
        static Color storedColor;

        public static void DrawBounds(Bounds b) {
            Gizmos.DrawWireCube(b.center, b.size);
        }

        /// <summary>
        /// Stores the current Gizmos color and switches to a new color.
        /// Used in conjunction with <see cref="RestoreColor"/>
        /// </summary>
        /// <param name="color"></param>
        public static void StoreColor(Color color) {
            storedColor = Gizmos.color;
            Gizmos.color = color;
        }

        /// <summary>
        /// Restores the stored Gizmos color from <see cref="StoreColor(Color)"/>
        /// </summary>
        public static void RestoreColor() {
            Gizmos.color = storedColor;
        }

    }
}
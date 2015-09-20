using UnityEditor;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class HandlesUtils {
        static Color stored;

        public static void StoreColor(Color color) {
            stored = Handles.color;
            Handles.color = color;
        }

        public static void RestoreColor() {
            Handles.color = stored;
        }
    }
}
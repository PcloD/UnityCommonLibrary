using UnityEditor;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class HandlesUtility {
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
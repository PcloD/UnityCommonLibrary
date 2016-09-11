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
    }
}
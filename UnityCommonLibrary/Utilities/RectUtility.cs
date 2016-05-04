using UnityEngine;

namespace UnityCommonLibrary.Utilities
{
    public static class RectUtility {

        public static Vector2 ClosestPointOnRect(this Rect rect, Vector2 point) {
            Vector2 adjusted = Vector2.zero;
            adjusted.x = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
            adjusted.y = Mathf.Clamp(point.y, rect.yMin, rect.yMax);
            return adjusted;
        }

    }
}
using UnityEngine;

namespace UnityCommonLibrary.Utility
{
	public static class UIUtility
	{
		public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
		{
			var center = rectTransform.GetWorldCenter();
			rectTransform.pivot = pivot;
			rectTransform.position += (Vector3)(center - rectTransform.GetWorldCenter());
		}
		public static Vector2 GetWorldCenter(this RectTransform rectTransform)
		{
			return rectTransform.TransformPoint(rectTransform.rect.center);
		}
		public static Rect GetWorldRect(this RectTransform rectTransform)
		{
			return new Rect(rectTransform.TransformPoint(rectTransform.rect.min), rectTransform.rect.size);
		}
	}
}

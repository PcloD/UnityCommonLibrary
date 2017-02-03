using UnityEngine;

namespace UnityCommonLibrary
{
	public struct TransformSettings
	{
		public int siblingIndex { get; private set; }
		public Transform parent { get; private set; }
		public Vector3 position { get; private set; }
		public Quaternion rotation { get; private set; }
		public Vector3 localScale { get; private set; }

		public TransformSettings(Transform transform)
		{
			parent = transform.parent;
			siblingIndex = transform.GetSiblingIndex();
			position = transform.position;
			rotation = transform.rotation;
			localScale = transform.localScale;
		}

		public void ApplyTo(Transform transform)
		{
			transform.parent = parent;
			transform.SetSiblingIndex(siblingIndex);
			transform.position = position;
			transform.rotation = rotation;
			transform.localScale = localScale;
		}
	}

	public struct RectTransformSettings
	{
		private TransformSettings transformSettings;

		public Vector3 anchoredPosition3D { get; private set; }
		public Vector2 anchorMin { get; private set; }
		public Vector2 anchorMax { get; private set; }
		public Vector2 sizeDelta { get; private set; }
		public Vector2 pivot { get; private set; }


		public RectTransformSettings(RectTransform rect)
		{
			anchoredPosition3D = rect.anchoredPosition3D;
			anchorMin = rect.anchorMin;
			anchorMax = rect.anchorMax;
			sizeDelta = rect.sizeDelta;
			pivot = rect.pivot;
			transformSettings = new TransformSettings(rect);
		}

		public void ApplyAll(RectTransform rect)
		{
			transformSettings.ApplyTo(rect);
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.pivot = pivot;
			rect.anchoredPosition3D = anchoredPosition3D;
			rect.sizeDelta = sizeDelta;
		}
	}
}

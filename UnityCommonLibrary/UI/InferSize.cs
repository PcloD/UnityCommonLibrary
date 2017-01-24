using UnityCommonLibrary.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(LayoutElement))]
	public class InferSize : MonoBehaviour
	{
		public Graphic targetGraphic;

		[Header("Rect Size")]
		[DisplayName("Use")]
		public bool useRectSize;
		[DisplayName("Offset")]
		public Vector2 rectSizeOffset;

		[Header("Min Size")]
		[DisplayName("Use")]
		public bool useMinSize;
		[DisplayName("Offset")]
		public Vector2 minSizeOffset;

		[Header("Preferred Size")]
		[DisplayName("Use")]
		public bool usePreferredSize;
		[DisplayName("Offset")]
		public Vector2 preferredSizeOffset;

		[Header("Flexible Size")]
		[DisplayName("Use")]
		public bool useFlexibleSize;
		[DisplayName("Offset")]
		public Vector2 flexibleSizeOffset;

		private LayoutElement layout;
		private RectTransform rt;

		private void Awake()
		{
			rt = GetComponent<RectTransform>();
			layout = GetComponent<LayoutElement>();
		}

		private void Update()
		{
			if(targetGraphic != null)
			{
				if(useRectSize)
				{
					rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetGraphic.rectTransform.rect.width + rectSizeOffset.x);
					rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetGraphic.rectTransform.rect.height + rectSizeOffset.y);
				}
				if(useMinSize)
				{
					layout.minWidth = LayoutUtility.GetMinWidth(targetGraphic.rectTransform) + minSizeOffset.x;
					layout.minHeight = LayoutUtility.GetMinHeight(targetGraphic.rectTransform) + minSizeOffset.y;
				}
				if(usePreferredSize)
				{
					layout.preferredWidth = LayoutUtility.GetPreferredWidth(targetGraphic.rectTransform) + preferredSizeOffset.x;
					layout.preferredHeight = LayoutUtility.GetPreferredHeight(targetGraphic.rectTransform) + preferredSizeOffset.y;
				}
				if(useFlexibleSize)
				{
					layout.flexibleWidth = LayoutUtility.GetFlexibleWidth(targetGraphic.rectTransform) + flexibleSizeOffset.x;
					layout.flexibleHeight = LayoutUtility.GetFlexibleHeight(targetGraphic.rectTransform) + flexibleSizeOffset.y;
				}
			}
		}
	}
}
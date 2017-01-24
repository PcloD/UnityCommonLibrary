using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary.UI
{
	[RequireComponent(typeof(Slider))]
	[ExecuteInEditMode]
	public class SliderEffects : MonoBehaviour
	{
		[SerializeField]
		private Text text;

		[SerializeField]
		private string formatStr = "{0}";

		[SerializeField]
		private Gradient fill = new Gradient();

		private Image fillImg;
		private Slider slider;

		private void Update()
		{
			if(slider == null)
			{
				slider = GetComponent<Slider>();
			}
			if(fillImg == null)
			{
				fillImg = slider.fillRect.GetComponent<Image>();
			}
			fillImg.color = fill.Evaluate(slider.normalizedValue);
			if(text != null)
			{
				try
				{
					text.text = string.Format(formatStr, slider.value);
				}
				catch(FormatException)
				{
					text.text = "INVALID FORMAT: FormatException";
				}
				catch(IndexOutOfRangeException)
				{
					text.text = "INVALID FORMAT: IndexOutOfRange";
				}
			}
		}
	}
}
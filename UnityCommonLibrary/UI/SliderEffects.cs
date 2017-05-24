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
        private Gradient _fill = new Gradient();

        private Image _fillImg;

        [SerializeField]
        private string _formatStr = "{0}";

        private Slider _slider;

        [SerializeField]
        private Text _text;

        private void Update()
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
            }
            if (_fillImg == null)
            {
                _fillImg = _slider.fillRect.GetComponent<Image>();
            }
            _fillImg.color = _fill.Evaluate(_slider.normalizedValue);
            if (_text != null)
            {
                try
                {
                    _text.text = string.Format(_formatStr, _slider.value);
                }
                catch (FormatException)
                {
                    _text.text = "INVALID FORMAT: FormatException";
                }
                catch (IndexOutOfRangeException)
                {
                    _text.text = "INVALID FORMAT: IndexOutOfRange";
                }
            }
        }
    }
}
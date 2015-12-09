using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Slider))]
    [ExecuteInEditMode]
    public class SliderEffects : MonoBehaviour {

        [SerializeField]
        Text text;

        [SerializeField]
        string formatStr = "{0}";

        [SerializeField]
        Gradient fill = new Gradient();

        Image fillImg;
        Slider slider;

        void Update() {
            if(slider == null) {
                slider = GetComponent<Slider>();
            }
            if(fillImg == null) {
                fillImg = slider.fillRect.GetComponent<Image>();
            }
            fillImg.color = fill.Evaluate(slider.normalizedValue);
            if(text != null) {
                try {
                    text.text = string.Format(formatStr, slider.value);
                }
                catch(FormatException) {
                    text.text = "INVALID FORMAT: FormatException";
                }
                catch(IndexOutOfRangeException) {
                    text.text = "INVALID FORMAT: IndexOutOfRange";
                }
            }
        }
    }
}
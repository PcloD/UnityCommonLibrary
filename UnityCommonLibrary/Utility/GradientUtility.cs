using UnityEngine;

namespace UnityCommonLibrary.Utilities {
    public static class GradientUtility {

        public static Gradient MakeRainbow(this Gradient g) {
            g.colorKeys = new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 0f, 0f), 0f),
                new GradientColorKey(new Color(1f, 1f, 0f), 0.25f),
                new GradientColorKey(new Color(0f, 1f, 0f), 0.5f),
                new GradientColorKey(new Color(0f, 1f, 1f), 0.75f),
                new GradientColorKey(new Color(1f, 0f, 1f), 1f),
            };
            g.alphaKeys = new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f)
            };
            return g;
        }

    }
}
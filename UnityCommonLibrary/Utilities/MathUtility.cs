using System;

namespace UnityCommonLibrary.Utilities {
    public static class MathUtility {

        /// <summary>
        /// Clamps a float to the -1...1 inclusive range
        /// </summary>
        /// <param name="f"></param>
        /// <returns>A normalized float.</returns>
        public static float Normalize(float f) {
            if(f > 1f) {
                return 1f;
            }
            else if(f < -1f) {
                return -1f;
            }
            else {
                return f;
            }
        }

        public static float SignOrZero(float f) {
            if(f == 0f) {
                return 0f;
            }
            else {
                return UnityEngine.Mathf.Sign(f);
            }
        }

        public static byte ClampByte(byte current, byte min, byte max) {
            if(current > max) {
                return max;
            }
            else if(current < min) {
                return min;
            }
            else {
                return current;
            }
        }

        public static sbyte ClampSByte(sbyte current, sbyte min, sbyte max) {
            if(current > max) {
                return max;
            }
            else if(current < min) {
                return min;
            }
            else {
                return current;
            }
        }

        public static float RoundTo(float f, float nearest) {
            var multiple = 1f / nearest;
            return (float)Math.Round(f * multiple, MidpointRounding.AwayFromZero) / multiple;
        }


        public static float Map(float value, float oldMin, float oldMax, float newMin, float newMax) {
            return (((value - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
        }
    }
}
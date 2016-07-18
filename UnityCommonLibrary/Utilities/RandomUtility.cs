using UnityEngine;

namespace UnityCommonLibrary.Utilities
{
    public static class RandomUtility
    {

        public static Vector4 Vector4(float min, float max)
        {
            return new Vector4(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        }

        public static Vector3 Vector3(float min, float max)
        {
            return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
        }

        public static Vector2 Vector2(float min, float max)
        {
            return new Vector2(Random.Range(min, max), Random.Range(min, max));
        }

        public static float NormalRange()
        {
            return Random.Range(0f, 1f);
        }

        public static float FullRange()
        {
            return Random.Range(-1f, 1f);
        }

    }
}
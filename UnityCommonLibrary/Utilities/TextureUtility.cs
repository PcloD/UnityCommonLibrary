using UnityEngine;

namespace UnityCommonLibrary.Utilities
{
    public static class TextureUtility
    {
        public static float GetAspect(this Texture texture)
        {
            return (float)texture.width / texture.height;
        }
    }
}
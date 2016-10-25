using UnityEngine;

namespace UnityCommonLibrary.Utility
{
    public static class TextureUtility
    {
        public static float GetAspect(this Texture texture)
        {
            return (float)texture.width / texture.height;
        }
    }
}
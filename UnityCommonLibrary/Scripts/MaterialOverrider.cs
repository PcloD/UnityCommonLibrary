using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    public static class MaterialOverrider
    {
        private static Dictionary<Renderer, Material> overriden = new Dictionary<Renderer, Material>();

        public static void Override(Renderer renderer, Material material)
        {
            if (!overriden.ContainsKey(renderer))
            {
                overriden.Add(renderer, renderer.sharedMaterial);
            }
            renderer.sharedMaterial = material;
        }
        public static void Restore(Renderer renderer)
        {
            Material original;
            if (overriden.TryGetValue(renderer, out original))
            {
                renderer.sharedMaterial = original;
                overriden.Remove(renderer);
            }
        }
        public static void RestoreAll()
        {
            var renderers = new Renderer[overriden.Count];
            int index = 0;
            foreach (var item in overriden)
            {
                renderers[index] = item.Key;
            }
            for (int i = 0; i < renderers.Length; i++)
            {
                Restore(renderers[i]);
            }
        }
        public static void Clear()
        {
            overriden.Clear();
        }
    }

}
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    public static class MaterialOverrider
    {
        private static readonly Dictionary<Renderer, Material> Overriden =
            new Dictionary<Renderer, Material>();

        public static void Override(Renderer renderer, Material material)
        {
            if (!Overriden.ContainsKey(renderer))
            {
                Overriden.Add(renderer, renderer.sharedMaterial);
            }
            renderer.sharedMaterial = material;
        }

        public static void Restore(Renderer renderer)
        {
            Material original;
            if (Overriden.TryGetValue(renderer, out original))
            {
                renderer.sharedMaterial = original;
                Overriden.Remove(renderer);
            }
        }

        public static void RestoreAll()
        {
            var renderers = new Renderer[Overriden.Count];
            var index = 0;
            foreach (var item in Overriden)
            {
                renderers[index] = item.Key;
            }
            for (var i = 0; i < renderers.Length; i++)
            {
                Restore(renderers[i]);
            }
        }

        public static void Clear()
        {
            Overriden.Clear();
        }
    }
}
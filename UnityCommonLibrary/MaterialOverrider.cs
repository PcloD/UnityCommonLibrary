using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    public static class MaterialOverrider
    {
        private static readonly Dictionary<Renderer, Material> _overriden =
            new Dictionary<Renderer, Material>();

        public static void Override(Renderer renderer, Material material)
        {
            if (!_overriden.ContainsKey(renderer))
            {
                _overriden.Add(renderer, renderer.sharedMaterial);
            }
            renderer.sharedMaterial = material;
        }

        public static void Restore(Renderer renderer)
        {
            Material original;
            if (_overriden.TryGetValue(renderer, out original))
            {
                renderer.sharedMaterial = original;
                _overriden.Remove(renderer);
            }
        }

        public static void RestoreAll()
        {
            var renderers = new Renderer[_overriden.Count];
            var index = 0;
            foreach (var item in _overriden)
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
            _overriden.Clear();
        }
    }
}
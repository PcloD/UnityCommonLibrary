using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Utility
{
    public static class CoroutineUtility
    {
        private static readonly Dictionary<string, Coroutine> KeyedRoutines =
            new Dictionary<string, Coroutine>();

        private static EmptyMonoBehaviour _surrogate;

        private static EmptyMonoBehaviour Surrogate
        {
            get
            {
                EnsureSurrogate();
                return _surrogate;
            }
        }

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return Surrogate.StartCoroutine(routine);
        }

        public static Coroutine StartCoroutine(string key, IEnumerator routine)
        {
            var coroutine = Surrogate.StartCoroutine(routine);
            KeyedRoutines.Add(key, coroutine);
            return coroutine;
        }

        public static void StopAllCoroutines()
        {
            Surrogate.StopAllCoroutines();
            KeyedRoutines.Clear();
        }

        public static void StopKeyedCoroutines()
        {
            foreach (var coroutine in KeyedRoutines.Values)
            {
                StopCoroutine(coroutine);
            }
            KeyedRoutines.Clear();
        }

        public static void StopCoroutine(IEnumerator routine)
        {
            Surrogate.StopCoroutine(routine);
        }

        public static void StopCoroutine(Coroutine routine)
        {
            Surrogate.StopCoroutine(routine);
        }

        public static void StopCoroutine(string key)
        {
            Coroutine routine;
            if (KeyedRoutines.TryGetValue(key, out routine))
            {
                Surrogate.StopCoroutine(routine);
            }
        }

        private static void EnsureSurrogate()
        {
            if (!_surrogate)
            {
                _surrogate =
                    ComponentUtility.Create<EmptyMonoBehaviour>(
                        "CoroutineUtilitySurrogate");
                _surrogate.hideFlags = HideFlags.NotEditable;
                Object.DontDestroyOnLoad(_surrogate);
            }
        }
    }
}
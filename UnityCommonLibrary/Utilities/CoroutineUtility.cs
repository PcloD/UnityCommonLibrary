using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Utilities
{

    public static class CoroutineUtility
    {
        private static EmptyMonoBehaviour _surrogate;
        private static EmptyMonoBehaviour surrogate
        {
            get
            {
                EnsureSurrogate();
                return _surrogate;
            }
        }
        private static Dictionary<string, Coroutine> keyedRoutines = new Dictionary<string, Coroutine>();

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return surrogate.StartCoroutine(routine);
        }
        public static Coroutine StartCoroutine(string key, IEnumerator routine)
        {
            var coroutine = surrogate.StartCoroutine(routine);
            keyedRoutines.Add(key, coroutine);
            return coroutine;
        }
        public static void StopAllCoroutines()
        {
            surrogate.StopAllCoroutines();
            keyedRoutines.Clear();
        }
        public static void StopKeyedCoroutines()
        {
            foreach (var coroutine in keyedRoutines.Values)
            {
                StopCoroutine(coroutine);
            }
            keyedRoutines.Clear();
        }
        public static void StopCoroutine(IEnumerator routine)
        {
            surrogate.StopCoroutine(routine);
        }
        public static void StopCoroutine(Coroutine routine)
        {
            surrogate.StopCoroutine(routine);
        }
        public static void StopCoroutine(string key)
        {
            Coroutine routine;
            if (keyedRoutines.TryGetValue(key, out routine))
            {
                surrogate.StopCoroutine(routine);
            }
        }
        private static void EnsureSurrogate()
        {
            if (!_surrogate)
            {
                _surrogate = ComponentUtility.Create<EmptyMonoBehaviour>("CoroutineUtilitySurrogate");
                _surrogate.hideFlags = HideFlags.NotEditable;
                Object.DontDestroyOnLoad(_surrogate);
            }
        }
    }
}
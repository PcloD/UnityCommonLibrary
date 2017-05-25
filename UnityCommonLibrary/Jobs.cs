using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    public class Jobs : MonoSingleton<Jobs>
    {
        private readonly List<Func<bool>> _onFixedUpdateJobs = new List<Func<bool>>();
        private readonly List<Func<bool>> _onLateUpdateJobs = new List<Func<bool>>();
        private readonly Queue<Action> _onUnityThreadJobs = new Queue<Action>();
        private readonly List<Func<bool>> _onUpdateJobs = new List<Func<bool>>();

        public static void ExecuteOnUnityThread(Action a)
        {
            Instance._onUnityThreadJobs.Enqueue(a);
        }

        public static void ExecuteOnUpdate(Func<bool> func)
        {
            Instance._onUpdateJobs.Add(func);
        }

        public static void ExecuteOnFixedUpdate(Func<bool> func)
        {
            Instance._onFixedUpdateJobs.Add(func);
        }

        public static void ExecuteOnLateUpdate(Func<bool> func)
        {
            Instance._onLateUpdateJobs.Add(func);
        }

        public static void ExecuteCoroutine(IEnumerator routine)
        {
            Instance.StartCoroutine(routine);
        }

        public static void ExecuteDelayed(float delay, Action action)
        {
            Instance.StartCoroutine(_ExecuteDelayed(delay, action));
        }

        public static void ExecuteNextFrame(Action action)
        {
            Instance.StartCoroutine(_ExecuteInFrames(1, action));
        }

        public static void ExecuteInFrames(int frames, Action action)
        {
            Instance.StartCoroutine(_ExecuteInFrames(frames, action));
        }

        private static IEnumerator _ExecuteDelayed(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        private static IEnumerator _ExecuteInFrames(int frames, Action action)
        {
            for (var i = 0; i < frames; i++)
            {
                yield return null;
            }
            action();
        }

        private void FixedUpdate()
        {
            for (var i = _onFixedUpdateJobs.Count - 1; i >= 0; i--)
            {
                var job = _onFixedUpdateJobs[i];
                if (job == null && !job.Method.IsStatic || !job())
                {
                    _onFixedUpdateJobs.RemoveAt(i);
                }
            }
        }

        private void LateUpdate()
        {
            for (var i = _onLateUpdateJobs.Count - 1; i >= 0; i--)
            {
                var job = _onLateUpdateJobs[i];
                if (job == null && !job.Method.IsStatic || !job())
                {
                    _onLateUpdateJobs.RemoveAt(i);
                }
            }
        }

        private void Update()
        {
            while (_onUnityThreadJobs.Count > 0)
            {
                var job = _onUnityThreadJobs.Dequeue();
                if (job != null)
                {
                    job();
                }
            }
            for (var i = _onUpdateJobs.Count - 1; i >= 0; i--)
            {
                var job = _onUpdateJobs[i];
                if (job == null && !job.Method.IsStatic || !job())
                {
                    _onUpdateJobs.RemoveAt(i);
                }
            }
        }
    }
}
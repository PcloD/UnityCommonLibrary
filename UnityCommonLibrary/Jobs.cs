using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    public class Jobs : MonoSingleton<Jobs>
    {
        private readonly Dictionary<Guid, Action> _onFixedUpdateJobs
            = new Dictionary<Guid, Action>();
        private readonly List<Guid> _onFixedUpdateRemovals =
            new List<Guid>();
        private readonly Dictionary<Guid, Action> _onLateUpdateJobs
            = new Dictionary<Guid, Action>();
        private readonly List<Guid> _onLateUpdateRemovals =
            new List<Guid>();
        private readonly Dictionary<Guid, Action> _onUpdateJobs
            = new Dictionary<Guid, Action>();
        private readonly List<Guid> _onUpdateRemovals =
            new List<Guid>();
        private readonly Queue<Action> _onUnityThreadJobs = new Queue<Action>();

        public static void ExecuteOnUnityThread(Action a)
        {
            Instance._onUnityThreadJobs.Enqueue(a);
        }

        public static Guid ExecuteOnUpdate(Action onUpdate)
        {
            return AddNewEntry(onUpdate, Instance._onUpdateJobs);
        }

        public static Guid ExecuteOnFixedUpdate(Action onFixedUpdate)
        {
            return AddNewEntry(onFixedUpdate, Instance._onFixedUpdateJobs);
        }

        public static Guid ExecuteOnLateUpdate(Action onLateUpdate)
        {
            return AddNewEntry(onLateUpdate, Instance._onLateUpdateJobs);
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

        private static Guid AddNewEntry(Action onUpdate, Dictionary<Guid, Action> onUpdateJobs)
        {
            var guid = Guid.NewGuid();
            onUpdateJobs.Add(guid, onUpdate);
            return guid;
        }

        private static void TickLists(List<Guid> removals, Dictionary<Guid, Action> jobs)
        {
            foreach (var guid in removals)
            {
                jobs.Remove(guid);
            }
            foreach (var kvp in jobs)
            {
                if (kvp.Value == null)
                {
                    removals.Add(kvp.Key);
                }
                else
                {
                    kvp.Value();
                }
            }
        }

        private void FixedUpdate()
        {
            TickLists(_onFixedUpdateRemovals, _onFixedUpdateJobs);
        }

        private void LateUpdate()
        {
            TickLists(_onLateUpdateRemovals, _onLateUpdateJobs);
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
            TickLists(_onUpdateRemovals, _onUpdateJobs);
        }
    }
}
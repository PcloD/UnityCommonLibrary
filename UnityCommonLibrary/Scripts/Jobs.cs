using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
	public class Jobs : MonoSingleton<Jobs>
	{
		private Queue<Action> onUnityThreadJobs = new Queue<Action>();
		private List<Func<bool>> onUpdateJobs = new List<Func<bool>>();
		private List<Func<bool>> onFixedUpdateJobs = new List<Func<bool>>();
		private List<Func<bool>> onLateUpdateJobs = new List<Func<bool>>();

		public static void ExecuteOnUnityThread(Action a)
		{
			Instance.onUnityThreadJobs.Enqueue(a);
		}
		public static void ExecuteOnUpdate(Func<bool> func)
		{
			Instance.onUpdateJobs.Add(func);
		}
		public static void ExecuteOnFixedUpdate(Func<bool> func)
		{
			Instance.onFixedUpdateJobs.Add(func);
		}
		public static void ExecuteOnLateUpdate(Func<bool> func)
		{
			Instance.onLateUpdateJobs.Add(func);
		}
		public static void ExecuteCoroutine(IEnumerator routine)
		{
			Instance.StartCoroutine(routine);
		}
		public static void ExecuteDelayed(float delay, Action action)
		{
			Instance.StartCoroutine(Instance._ExecuteDelayed(delay, action));
		}
		public static void ExecuteNextFrame(Action action)
		{
			Instance.StartCoroutine(Instance._ExecuteInFrames(1, action));
		}
		public static void ExecuteInFrames(int frames, Action action)
		{
			Instance.StartCoroutine(Instance._ExecuteInFrames(frames, action));
		}

		private IEnumerator _ExecuteDelayed(float delay, Action action)
		{
			yield return new WaitForSeconds(delay);
			action();
		}
		private IEnumerator _ExecuteInFrames(int frames, Action action)
		{
			for(int i = 0; i < frames; i++)
			{
				yield return null;
			}
			action();
		}
		private void Update()
		{
			while(onUnityThreadJobs.Count > 0)
			{
				var count = onUnityThreadJobs.Count;
				var job = onUnityThreadJobs.Dequeue();
				// Check for infinite loop from a callback adding itself back
				if(job != null)
				{
					job();
				}
				if(onUnityThreadJobs.Count == count)
				{
					Debug.LogFormat("{0} on {1} added job to onthread queue!", job.Method, job.Target);
					break;
				}
			}
			for(int i = onUpdateJobs.Count - 1; i >= 0; i--)
			{
				var job = onUpdateJobs[i];
				if((job == null && !job.Method.IsStatic) || !job())
				{
					onUpdateJobs.RemoveAt(i);
				}
			}
		}
		private void FixedUpdate()
		{
			for(int i = onFixedUpdateJobs.Count - 1; i >= 0; i--)
			{
				var job = onFixedUpdateJobs[i];
				if((job == null && !job.Method.IsStatic) || !job())
				{
					onFixedUpdateJobs.RemoveAt(i);
				}
			}
		}
		private void LateUpdate()
		{
			for(int i = onLateUpdateJobs.Count - 1; i >= 0; i--)
			{
				var job = onLateUpdateJobs[i];
				if((job == null && !job.Method.IsStatic) || !job())
				{
					onLateUpdateJobs.RemoveAt(i);
				}
			}
		}
	}
}
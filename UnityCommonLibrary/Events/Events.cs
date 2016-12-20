using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Events
{
	public delegate void OnEvent(EventData data);
	public static class Events<T> where T : struct, IFormattable, IConvertible, IComparable
	{
		internal static HashSet<object> toRemove = new HashSet<object>();
		private static Dictionary<T, HashSet<OnEvent>> listeners = new Dictionary<T, HashSet<OnEvent>>();
		private static T[] allEvents;
		private static EventsRunner runner;

		static Events()
		{
			if(!Application.isPlaying)
			{
				return;
			}
			if(!typeof(T).IsEnum)
			{
				throw new Exception("Type T must be enum.");
			}
			allEvents = (T[])Enum.GetValues(typeof(T));
			runner = Utility.ComponentUtility.Create<EventsRunner>(string.Format("{0}_EventRunner", typeof(T).Name));
			UnityEngine.Object.DontDestroyOnLoad(runner);
			runner.getListeners = (e) =>
			{
				var set = listeners[FromEnum(e)];
				set.RemoveWhere(l => l == null || l.Target == null);
				return set;
			};
			runner.doPendingRemovals = RemovePending;
			for(int i = 0; i < allEvents.Length; i++)
			{
				listeners.Add(allEvents[i], new HashSet<OnEvent>());
			}
		}

		public static void Broadcast(T ge, EventData data = null)
		{
			if(!Application.isPlaying)
			{
				return;
			}
			if(data != null)
			{
				data.isLocked = true;
			}
			runner.Enqueue(new EventsRunner.EventCall()
			{
				eventType = ToEnum(ge),
				data = data,
			});
		}
		public static void Register(T evt, OnEvent callback)
		{
			if(!Application.isPlaying)
			{
				return;
			}
			HashSet<OnEvent> set;
			if(!listeners.TryGetValue(evt, out set))
			{
				set = new HashSet<OnEvent>();
				listeners[evt] = set;
			}
			set.Add(callback);
		}
		public static void RemoveAll(object target)
		{
			if(!Application.isPlaying)
			{
				return;
			}
			toRemove.Add(target);
		}
		public static void Remove(T evt, object target)
		{
			if(!Application.isPlaying)
			{
				return;
			}
			listeners[evt].RemoveWhere(v => v.Target == target);
		}
		public static T FromEnum(Enum e)
		{
			return (T)(object)e;
		}
		public static Enum ToEnum(T e)
		{
			return (Enum)(object)e;
		}
		private static void RemovePending()
		{
			if(!Application.isPlaying)
			{
				return;
			}
			foreach(var k in listeners.Keys)
			{
				foreach(var o in toRemove)
				{
					Remove(k, o);
				}
			}
			toRemove.Clear();
		}
	}
}
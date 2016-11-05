using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Events
{
	public delegate void OnEvent(EventData data);
	public static class Events<T> where T : struct, IFormattable, IConvertible, IComparable
	{

		internal static Dictionary<T, HashSet<OnEvent>> listeners = new Dictionary<T, HashSet<OnEvent>>();
		private static T[] allEvents;
		private static EventsRunner runner;

		static Events()
		{
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
			for(int i = 0; i < allEvents.Length; i++)
			{
				listeners.Add(allEvents[i], new HashSet<OnEvent>());
			}
		}

		public static void Broadcast(T ge, EventData data = null)
		{
			data.isLocked = true;
			runner.Enqueue(new EventsRunner.EventCall()
			{
				eventType = ToEnum(ge),
				data = data,
			});
		}
		public static void Register(T evt, OnEvent callback)
		{
			HashSet<OnEvent> list;
			if(!listeners.TryGetValue(evt, out list))
			{
				list = new HashSet<OnEvent>();
				listeners[evt] = list;
			}
			list.Add(callback);
		}
		public static void RemoveAll(object target)
		{
			foreach(var kvp in listeners)
			{
				Remove(kvp.Key, target);
			}
		}
		public static void Remove(T evt, object target)
		{
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
	}
}
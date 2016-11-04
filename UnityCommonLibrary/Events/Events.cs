using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Events
{
	public delegate void OnEvent(EventData data);
	public static class Events<T> where T : struct, IFormattable, IConvertible, IComparable
	{

		internal static Dictionary<T, List<OnEvent>> listeners = new Dictionary<T, List<OnEvent>>();
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
				var list = listeners[FromEnum(e)];
				list.RemoveAll(l => l == null || l.Target == null);
				return list;
			};
			for(int i = 0; i < allEvents.Length; i++)
			{
				listeners.Add(allEvents[i], new List<OnEvent>());
			}
		}

		public static void Broadcast(T ge, EventData data = null)
		{
			data.isLocked = true;
			runner.rawQueue.Enqueue(new EventsRunner.EventCall()
			{
				eventType = ToEnum(ge),
				data = data,
			});
		}
		public static void Register(T evt, OnEvent callback)
		{
			List<OnEvent> list;
			if(!listeners.TryGetValue(evt, out list))
			{
				list = new List<OnEvent>();
				listeners[evt] = list;
			}
			list.Add(callback);
		}
		public static void Remove(object target)
		{
			foreach(var kvp in listeners)
			{
				for(int i = kvp.Value.Count - 1; i >= 0; i--)
				{
					if(kvp.Value[i].Target == target)
					{
						kvp.Value.RemoveAt(i);
					}
				}
			}
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
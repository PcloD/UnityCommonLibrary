using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Events
{
	public static class Events<T> where T : struct, IFormattable, IConvertible, IComparable
	{
		private static Dictionary<T, List<IEventListener<T>>> listeners = new Dictionary<T, List<IEventListener<T>>>();
		private static T[] allEvents;

		static Events()
		{
			if (!typeof(T).IsEnum)
			{
				throw new Exception("Type T must be enum.");
			}
			allEvents = (T[])Enum.GetValues(typeof(T));
		}

		public static void Broadcast(T ge, EventData data = null)
		{
			data.isLocked = true;
			List<IEventListener<T>> list;
			if (listeners.TryGetValue(ge, out list) && list.Count > 0)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (list[i] == null)
					{
						list.RemoveAt(i);
					}
					else
					{
						list[i].OnEvent(ge, data);
					}
				}
			}
		}
		public static void Register(IEventListener<T> listener, params T[] events)
		{
			for (int i = 0; i < events.Length; i++)
			{
				var evt = events[i];
				List<IEventListener<T>> list;
				if (!listeners.TryGetValue(evt, out list))
				{
					list = new List<IEventListener<T>>();
					listeners[evt] = list;
				}
				list.Add(listener);
			}
		}
		public static void RegisterAll(IEventListener<T> listener)
		{
			for (int i = 0; i < allEvents.Length; i++)
			{
				Register(listener, allEvents[i]);
			}
		}

	}
}

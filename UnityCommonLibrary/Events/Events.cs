using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Events
{
	public static class Events<T> where T : struct, IFormattable, IConvertible, IComparable
	{
		public delegate void OnEvent(EventData data);

		private static Dictionary<T, List<OnEvent>> listeners = new Dictionary<T, List<OnEvent>>();
		private static T[] allEvents;

		static Events()
		{
			if(!typeof(T).IsEnum)
			{
				throw new Exception("Type T must be enum.");
			}
			allEvents = (T[])Enum.GetValues(typeof(T));
		}

		public static void Broadcast(T ge, EventData data = null)
		{
			data.isLocked = true;
			List<OnEvent> list;
			if(listeners.TryGetValue(ge, out list) && list.Count > 0)
			{
				for(int i = list.Count - 1; i >= 0; i--)
				{
					if(list[i] == null || list[i].Target == null)
					{
						list.RemoveAt(i);
					}
					else
					{
						list[i](data);
					}
				}
			}
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
	}
}

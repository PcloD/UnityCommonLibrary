using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Events
{
	internal sealed class EventsRunner : MonoBehaviour
	{
		internal struct EventCall
		{
			public Enum eventType;
			public EventData data;
		}

		internal Func<Enum, List<OnEvent>> getListeners;
		internal Queue<EventCall> rawQueue = new Queue<EventCall>();

		private void Update()
		{
			while (rawQueue.Count > 0)
			{
				var evt = rawQueue.Dequeue();
				var callbacks = getListeners(evt.eventType);
				for (int i = 0; i < callbacks.Count; i++)
				{
					callbacks[i](evt.data);
				}
			}
		}
	}
}

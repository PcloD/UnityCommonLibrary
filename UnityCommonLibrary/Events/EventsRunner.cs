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

		internal Func<Enum, HashSet<OnEvent>> getListeners;
		private bool isExecutingQueue;
		private Queue<EventCall> primaryQueue = new Queue<EventCall>();
		private Queue<EventCall> secondaryQueue = new Queue<EventCall>();

		internal void Enqueue(EventCall evt)
		{
			(isExecutingQueue ? secondaryQueue : primaryQueue).Enqueue(evt);
		}
		private void Update()
		{
			isExecutingQueue = true;
			while(primaryQueue.Count > 0)
			{
				var evt = primaryQueue.Dequeue();
				var callbacks = getListeners(evt.eventType);
				foreach(var cb in callbacks)
				{
					cb(evt.data);
				}
			}
			isExecutingQueue = false;
			while(secondaryQueue.Count > 0)
			{
				primaryQueue.Enqueue(secondaryQueue.Dequeue());
			}
		}
	}
}

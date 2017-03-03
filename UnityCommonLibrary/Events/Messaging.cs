using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Messaging
{
	public delegate void OnMessage(IMessageData iData);

	public static class Messaging<M> where M : struct, IFormattable, IConvertible, IComparable
	{
		private struct MessageCall
		{
			public M messageType;
			public IMessageData data;
		}

		private static readonly HashSet<object> toRemove = new HashSet<object>();
		private static readonly Dictionary<M, HashSet<OnMessage>> listeners = new Dictionary<M, HashSet<OnMessage>>();
		private static readonly Queue<MessageCall> primaryQueue = new Queue<MessageCall>();
		private static readonly Queue<MessageCall> addQueue = new Queue<MessageCall>();
		private static bool isExecutingQueue;

		static Messaging()
		{
			if(!typeof(M).IsEnum)
			{
				throw new Exception("Type M must be enum.");
			}
			for(int i = 0; i < EnumData<M>.count; i++)
			{
				listeners.Add(EnumData<M>.values[i], new HashSet<OnMessage>());
			}
		}

		public static void Update()
		{
			isExecutingQueue = true;
			while(primaryQueue.Count > 0)
			{
				var evt = primaryQueue.Dequeue();
				ExecuteMessage(evt);
			}
			isExecutingQueue = false;
			PostInternalUpdateCleanup();
		}
		public static void Broadcast(M msg, IMessageData data = null)
		{
			(isExecutingQueue ? addQueue : primaryQueue).Enqueue(PrepareBroadcast(msg, data));
		}
		public static void BroadcastImmediate(M msg, IMessageData data = null)
		{
			var message = PrepareBroadcast(msg, data);
			// Force update
			ExecuteMessage(message);
			PostInternalUpdateCleanup();
		}
		public static void Register(M evt, OnMessage callback)
		{
			HashSet<OnMessage> set;
			if(!listeners.TryGetValue(evt, out set))
			{
				set = new HashSet<OnMessage>();
				listeners[evt] = set;
			}
			set.Add(callback);
		}
		public static void RemoveAll(object target)
		{
			toRemove.Add(target);
		}
		private static MessageCall PrepareBroadcast(M msg, IMessageData data)
		{
			if(data != null)
			{
				data.OnBroadcast();
			}
			return new MessageCall()
			{
				messageType = msg,
				data = data,
			};
		}
		private static void ExecuteMessage(MessageCall evt)
		{
			var callbacks = listeners[evt.messageType];
			callbacks.RemoveWhere(cb => cb.Target == null && !cb.Method.IsStatic);
			foreach(var cb in callbacks)
			{
				cb(evt.data);
			}
		}
		private static void PostInternalUpdateCleanup()
		{
			while(addQueue.Count > 0)
			{
				primaryQueue.Enqueue(addQueue.Dequeue());
			}
			foreach(var k in listeners.Keys)
			{
				foreach(var o in toRemove)
				{
					listeners[k].RemoveWhere(v =>
					{
						return Equals(v.Target, o);
					});
				}
			}
			toRemove.Clear();
		}
	}
}
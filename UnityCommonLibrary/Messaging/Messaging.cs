using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary.Messaging
{
    public delegate void OnMessage(MessageData abstData);

    public static class Messaging<M> where M : struct, IFormattable, IConvertible, IComparable
    {
        /// <summary>
        /// Pairs a specific message call to the
        /// data that was passed with it.
        /// </summary>
		private struct MessageCall
        {
            public M messageType;
            public MessageData data;
        }

        /// <summary>
        /// Binds message types to registered callbacks
        /// </summary>
        private static readonly Dictionary<M, HashSet<OnMessage>> listeners = new Dictionary<M, HashSet<OnMessage>>();
        /// <summary>
        /// Standard queue that broadcasted messages are added to.
        /// </summary>
        private static readonly Queue<MessageCall> primaryQueue = new Queue<MessageCall>();
        /// <summary>
        /// Secondary queue used if callbacks broadcast messages themselves.
        /// </summary>
        private static readonly Queue<MessageCall> secondayQueue = new Queue<MessageCall>();
        /// <summary>
        /// Flag to prevent infinite loops if callbacks broadcast messages themselves.
        /// </summary>
        private static bool updating;

        static Messaging()
        {
            if (!typeof(M).IsEnum)
            {
                throw new Exception("Type M must be enum.");
            }
            for (int i = 0; i < EnumData<M>.count; i++)
            {
                listeners.Add(EnumData<M>.values[i], new HashSet<OnMessage>());
            }
        }

        public static void Update()
        {
            updating = true;
            while (primaryQueue.Count > 0)
            {
                var evt = primaryQueue.Dequeue();
                ExecuteMessage(evt);
            }
            updating = false;
            PostUpdateCleanup();
        }
        /// <summary>
        /// Queues a Message for broadcasting.
        /// </summary>
        public static void Broadcast(M msg, MessageData data = null)
        {
            (updating ? secondayQueue : primaryQueue).Enqueue(CreateBroadcastMessage(msg, data));
        }
        /// <summary>
        /// Immediately broadcasts a message.
        /// </summary>
        public static void BroadcastImmediate(M msg, MessageData data = null)
        {
            var message = CreateBroadcastMessage(msg, data);
            // Force update
            updating = true;
            ExecuteMessage(message);
            PostUpdateCleanup();
            updating = false;
        }
        public static void Register(M evt, OnMessage callback)
        {
            HashSet<OnMessage> set;
            if (!listeners.TryGetValue(evt, out set))
            {
                set = new HashSet<OnMessage>();
                listeners[evt] = set;
            }
            set.Add(callback);
        }
        /// <summary>
        /// Removes all callbacks associated with target.
        /// </summary>
        public static void RemoveAll(object target)
        {
            for (int i = 0; i < EnumData<M>.count; i++)
            {
                listeners[EnumData<M>.values[i]].RemoveWhere(v =>
                {
                    return Equals(v.Target, target);
                });
            }
        }
        private static MessageCall CreateBroadcastMessage(M msg, MessageData data)
        {
            if (data != null)
            {
                data.PrepareBroadcast();
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
            // Remove any callbacks in which the non-static target no longer exists
            callbacks.RemoveWhere(cb => cb.Target == null && !cb.Method.IsStatic);
            foreach (var cb in callbacks)
            {
                cb(evt.data);
            }
        }
        private static void PostUpdateCleanup()
        {
            // Move secondary queue elements to primary queue
            while (secondayQueue.Count > 0)
            {
                primaryQueue.Enqueue(secondayQueue.Dequeue());
            }
        }
    }
}
using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
    public interface IEvent
    {
        void RemoveTarget(object target);
        void Update();
    }
    public abstract class BaseEvent<T> : IEvent
    {
        protected readonly HashSet<T> listeners = new HashSet<T>();

        public BaseEvent()
        {
            Events.events.Add(this);
        }

        public void Subscribe(T callback)
        {
            listeners.Add(callback);
        }
        public void Remove(T callback)
        {
            listeners.Remove(callback);
        }
        public void RemoveTarget(object target)
        {
            listeners.RemoveWhere(t => (t as Delegate).Target == target);
        }
        public abstract void Update();
    }

    public class Event : BaseEvent<Event.OnEvent>
    {
        public delegate void OnEvent();

        private int callQueue;

        public override void Update()
        {
            if (callQueue > 0)
            {
                listeners.RemoveWhere(Events.CheckRemoveCallback);
                for (int i = 0; i < callQueue; i++)
                {
                    InternalBroadcast();
                }
                callQueue = 0;
            }
        }
        public void Broadcast()
        {
            callQueue++;
        }
        public void BroadcastImmediate()
        {
            listeners.RemoveWhere(Events.CheckRemoveCallback);
            InternalBroadcast();
        }
        private void InternalBroadcast()
        {
            foreach (var l in listeners)
            {
                l.Invoke();
            }
        }
    }
    public class Event<T> : BaseEvent<Event<T>.OnEvent>
    {
        public delegate void OnEvent(T arg);

        private readonly Queue<T> callQueue = new Queue<T>();

        public override void Update()
        {
            if (callQueue.Count > 0)
            {
                listeners.RemoveWhere(Events.CheckRemoveCallback);
                while (callQueue.Count > 0)
                {
                    var call = callQueue.Dequeue();
                    InternalBroadcast(call);
                }
            }
        }
        public void Broadcast(T arg = default(T))
        {
            callQueue.Enqueue(arg);
        }
        public void BroadcastImmediate(T arg = default(T))
        {
            listeners.RemoveWhere(Events.CheckRemoveCallback);
            InternalBroadcast(arg);
        }
        private void InternalBroadcast(T arg)
        {
            foreach (var l in listeners)
            {
                l.Invoke(arg);
            }
        }
    }
    public class Event<T1, T2> : BaseEvent<Event<T1, T2>.OnEvent>
    {
        private class QueueEntry
        {
            public T1 arg1;
            public T2 arg2;

            public QueueEntry(T1 arg1, T2 arg2)
            {
                this.arg1 = arg1;
                this.arg2 = arg2;
            }
        }

        public delegate void OnEvent(T1 arg1, T2 arg2);

        private readonly Queue<QueueEntry> callQueue = new Queue<QueueEntry>();

        public override void Update()
        {
            if (callQueue.Count > 0)
            {
                listeners.RemoveWhere(Events.CheckRemoveCallback);
                while (callQueue.Count > 0)
                {
                    var call = callQueue.Dequeue();
                    InternalBroadcast(call.arg1, call.arg2);
                }
            }
        }
        public void Broadcast(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            callQueue.Enqueue(new QueueEntry(arg1, arg2));
        }
        public void BroadcastImmediate(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            listeners.RemoveWhere(Events.CheckRemoveCallback);
            InternalBroadcast(arg1, arg2);
        }
        private void InternalBroadcast(T1 arg1, T2 arg2)
        {
            foreach (var l in listeners)
            {
                l.Invoke(arg1, arg2);
            }
        }
    }
}

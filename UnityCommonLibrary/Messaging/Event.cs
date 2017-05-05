using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
    public interface IEvent
    {
        void UnsubscribeTarget(object target);
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
        public void Unsubscribe(T callback)
        {
            listeners.Remove(callback);
        }
        public void UnsubscribeTarget(object target)
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
                    InternalPublsh();
                }
                callQueue = 0;
            }
        }
        public void Publish()
        {
            callQueue++;
        }
        public void PublishImmediate()
        {
            listeners.RemoveWhere(Events.CheckRemoveCallback);
            InternalPublsh();
        }
        private void InternalPublsh()
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
                    InternalPublish(call);
                }
            }
        }
        public void Publish(T arg = default(T))
        {
            callQueue.Enqueue(arg);
        }
        public void PublishImmediate(T arg = default(T))
        {
            listeners.RemoveWhere(Events.CheckRemoveCallback);
            InternalPublish(arg);
        }
        private void InternalPublish(T arg)
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
                    InternalPublish(call.arg1, call.arg2);
                }
            }
        }
        public void Publish(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            callQueue.Enqueue(new QueueEntry(arg1, arg2));
        }
        public void PublishImmediate(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            listeners.RemoveWhere(Events.CheckRemoveCallback);
            InternalPublish(arg1, arg2);
        }
        private void InternalPublish(T1 arg1, T2 arg2)
        {
            foreach (var l in listeners)
            {
                l.Invoke(arg1, arg2);
            }
        }
    }
}

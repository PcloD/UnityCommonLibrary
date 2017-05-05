using System;
using System.Collections.Generic;
using System.Text;
using UnityCommonLibrary.Utility;

namespace UnityCommonLibrary.Messaging
{
    public interface IEvent
    {
        string subscriberList { get; }
        void UnsubscribeTarget(object target);
        void Update();
    }
    public abstract class BaseEvent<T> : IEvent where T : class
    {
        protected readonly HashSet<T> subscribers = new HashSet<T>();

        public string subscriberList
        {
            get
            {
                if (subscribers.Count == 0)
                {
                    return string.Empty;
                }
                var sb = new StringBuilder();
                foreach (var s in subscribers)
                {
                    var del = s as Delegate;
                    sb.AppendLineFormat("{0}.{1}", del.Target, del.Method.Name);
                }
                sb.TrimEnd();
                return sb.ToString();
            }
        }

        public BaseEvent()
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            {
                throw new ArgumentException("T must be of type Delegate.");
            }
            Events.events.Add(this);
        }

        protected static bool ShouldRemoveCallback(T t)
        {
            var del = t as Delegate;
            return del.Target == null && !del.Method.IsStatic;
        }

        public abstract void Update();
        public void Subscribe(T subscriber)
        {
            subscribers.Add(subscriber);
        }
        public void Unsubscribe(T subscriber)
        {
            subscribers.Remove(subscriber);
        }
        public void UnsubscribeTarget(object target)
        {
            subscribers.RemoveWhere(s => Equals((s as Delegate).Target, target));
        }
    }

    public class Event : BaseEvent<Event.OnEvent>
    {
        public delegate void OnEvent();

        private int callQueue;

        public override void Update()
        {
            if (callQueue > 0)
            {
                subscribers.RemoveWhere(ShouldRemoveCallback);
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
            subscribers.RemoveWhere(ShouldRemoveCallback);
            InternalPublsh();
        }
        private void InternalPublsh()
        {
            foreach (var s in subscribers)
            {
                s.Invoke();
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
                subscribers.RemoveWhere(ShouldRemoveCallback);
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
            subscribers.RemoveWhere(ShouldRemoveCallback);
            InternalPublish(arg);
        }
        private void InternalPublish(T arg)
        {
            foreach (var s in subscribers)
            {
                s.Invoke(arg);
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
                subscribers.RemoveWhere(ShouldRemoveCallback);
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
            subscribers.RemoveWhere(ShouldRemoveCallback);
            InternalPublish(arg1, arg2);
        }
        private void InternalPublish(T1 arg1, T2 arg2)
        {
            foreach (var s in subscribers)
            {
                s.Invoke(arg1, arg2);
            }
        }
    }
}

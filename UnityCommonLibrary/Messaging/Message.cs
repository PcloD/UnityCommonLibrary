using System;
using System.Collections.Generic;
using System.Text;
using UnityCommonLibrary.Utility;

namespace UnityCommonLibrary.Messaging
{
    public interface IMessage
    {
        string SubscriberList { get; }
        void UnsubscribeTarget(object target);
    }

    public abstract class BaseMessage<T> : IMessage where T : class
    {
        protected readonly HashSet<T> Subscribers = new HashSet<T>();

        public string SubscriberList
        {
            get
            {
                if (Subscribers.Count == 0)
                {
                    return "";
                }
                var sb = new StringBuilder();
                foreach (var s in Subscribers)
                {
                    var del = s as Delegate;
                    sb.AppendLineFormat("{0}.{1}", del.Target, del.Method.Name);
                }
                sb.TrimEnd();
                return sb.ToString();
            }
        }

        protected BaseMessage()
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            {
                throw new ArgumentException("T must be of StateType Delegate.");
            }
            Messages.AllSignals.Add(this);
        }

        ~BaseMessage()
        {
            Messages.AllSignals.Remove(this);
        }

        protected static bool ShouldRemoveCallback(T t)
        {
            var del = t as Delegate;
            return del.Target == null && !del.Method.IsStatic;
        }

        /// <summary>
        ///     Adds a subscriber to this signal.
        /// </summary>
        /// <param name="subscriber"></param>
        /// <param name="removeCheck">If true, don't add if it exists already.</param>
        public void Subscribe(T subscriber, bool removeCheck = true)
        {
            if (removeCheck)
            {
                Unsubscribe(subscriber);
            }
            Subscribers.Add(subscriber);
        }

        public void Unsubscribe(T subscriber)
        {
            Subscribers.Remove(subscriber);
        }

        public void UnsubscribeTarget(object target)
        {
            Subscribers.RemoveWhere(s => Equals((s as Delegate).Target, target));
        }
    }

    public class Message : BaseMessage<Action>
    {
        public void Publish()
        {
            Subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in Subscribers)
            {
                s.Invoke();
            }
        }
    }

    public class Message<T> : BaseMessage<Action<T>>
    {
        public void Publish(T arg = default(T))
        {
            Subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in Subscribers)
            {
                s.Invoke(arg);
            }
        }
    }

    public class Message<T1, T2> : BaseMessage<Action<T1, T2>>
    {
        public void Publish(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            Subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in Subscribers)
            {
                s.Invoke(arg1, arg2);
            }
        }
    }

    public class Message<T1, T2, T3> : BaseMessage<Action<T1, T2, T3>>
    {
        public void Publish(T1 arg1 = default(T1), T2 arg2 = default(T2),
            T3 arg3 = default(T3))
        {
            Subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in Subscribers)
            {
                s.Invoke(arg1, arg2, arg3);
            }
        }
    }

    public class Message<T1, T2, T3, T4> : BaseMessage<Action<T1, T2, T3, T4>>
    {
        public void Publish(T1 arg1 = default(T1), T2 arg2 = default(T2),
            T3 arg3 = default(T3), T4 arg4 = default(T4))
        {
            Subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in Subscribers)
            {
                s.Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }
}
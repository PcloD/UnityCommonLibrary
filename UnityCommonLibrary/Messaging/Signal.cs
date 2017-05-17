using System;
using System.Collections.Generic;
using System.Text;
using UnityCommonLibrary.Utility;

namespace UnityCommonLibrary.Messaging
{
    public interface ISignal
    {
        string subscriberList { get; }
        void UnsubscribeTarget(object target);
    }

    public abstract class BaseSignal<T> : ISignal where T : class
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

        protected static bool ShouldRemoveCallback(T t)
        {
            var del = t as Delegate;
            return del.Target == null && !del.Method.IsStatic;
        }

        public BaseSignal()
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate)))
            {
                throw new ArgumentException("T must be of type Delegate.");
            }
            Signals.signals.Add(this);
        }

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

    public class Signal : BaseSignal<Action>
    {
        public void Publish()
        {
            subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in subscribers)
            {
                s.Invoke();
            }
        }
    }

    public class Signal<T> : BaseSignal<Action<T>>
    {
        public void Publish(T arg = default(T))
        {
            subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in subscribers)
            {
                s.Invoke(arg);
            }
        }
    }

    public class Signal<T1, T2> : BaseSignal<Action<T1, T2>>
    {
        public void Publish(T1 arg1 = default(T1), T2 arg2 = default(T2))
        {
            subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in subscribers)
            {
                s.Invoke(arg1, arg2);
            }
        }
    }

    public class Signal<T1, T2, T3> : BaseSignal<Action<T1, T2, T3>>
    {
        public void Publish(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3))
        {
            subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in subscribers)
            {
                s.Invoke(arg1, arg2, arg3);
            }
        }
    }

    public class Signal<T1, T2, T3, T4> : BaseSignal<Action<T1, T2, T3, T4>>
    {
        public void Publish(T1 arg1 = default(T1), T2 arg2 = default(T2), T3 arg3 = default(T3), T4 arg4 = default(T4))
        {
            subscribers.RemoveWhere(ShouldRemoveCallback);
            foreach (var s in subscribers)
            {
                s.Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }
}

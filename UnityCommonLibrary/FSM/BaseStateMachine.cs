using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityCommonLibrary.Messaging;
using UnityCommonLibrary.Time;

namespace UnityCommonLibrary.FSM
{
    public abstract class BaseStateMachine<T, T1, T2>
        where T : struct, IFormattable, IConvertible, IComparable
        where T1 : class, ICloneable, ISerializable
        where T2 : class, ICloneable, ISerializable
    {
        public readonly Message<T, T> StateChanged = new Message<T, T>();

        protected readonly Dictionary<T, HashSet<T1>> OnStateEnter =
            new Dictionary<T, HashSet<T1>>();
        protected readonly Dictionary<T, HashSet<T2>> OnStateExit =
            new Dictionary<T, HashSet<T2>>();
        protected readonly Dictionary<T, HashSet<Action>> OnStateUpdate =
            new Dictionary<T, HashSet<Action>>();

        public T CurrentState { get; protected set; }
        public T PreviousState { get; protected set; }
        public TimeSlice StateEnterTime { get; protected set; }

        protected BaseStateMachine()
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be Enum");
            }
            if (!typeof(Delegate).IsAssignableFrom(typeof(T1)))
            {
                throw new Exception("T1 must be Delegate");
            }
            if (!typeof(Delegate).IsAssignableFrom(typeof(T2)))
            {
                throw new Exception("T2 must be Delegate");
            }
        }

        public abstract void RemoveCallbacks(object obj);

        public void Update()
        {
            HashSet<Action> callbacks;
            if (OnStateUpdate.TryGetValue(CurrentState, out callbacks))
            {
                foreach (var a in callbacks)
                {
                    a();
                }
            }
        }

        public void AddOnUpdate(T state, Action onUpdate)
        {
            HashSet<Action> callbacks;
            if (!OnStateUpdate.TryGetValue(state, out callbacks))
            {
                callbacks = new HashSet<Action>();
                OnStateUpdate.Add(state, callbacks);
            }
            callbacks.Add(onUpdate);
        }

        public void AddOnEnter(T state, T1 onEnter)
        {
            HashSet<T1> callbacks;
            if (!OnStateEnter.TryGetValue(state, out callbacks))
            {
                callbacks = new HashSet<T1>();
                OnStateEnter.Add(state, callbacks);
            }
            callbacks.Add(onEnter);
        }

        public void AddOnExit(T state, T2 onExit)
        {
            HashSet<T2> callbacks;
            if (!OnStateExit.TryGetValue(state, out callbacks))
            {
                callbacks = new HashSet<T2>();
                OnStateExit.Add(state, callbacks);
            }
            callbacks.Add(onExit);
        }
    }
}

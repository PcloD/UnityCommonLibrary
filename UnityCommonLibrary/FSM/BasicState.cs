using System.Collections;
using UnityCommonLibrary.Utilities;

namespace UnityCommonLibrary.FSM
{
    public sealed class BasicState : AbstractHPDAState
    {
        public delegate IEnumerator StateAsyncEvent(AbstractHPDAState otherState);
        public delegate void StateEvent(AbstractHPDAState otherState);
        public delegate void StateTick();

        private event StateAsyncEvent OnEnterAsync;
        private event StateAsyncEvent OnExitAsync;
        private event StateEvent OnEnter;
        private event StateEvent OnExit;
        private event StateTick OnTick;

        public BasicState(string id = null, bool useAsyncEnter = false, bool useAsyncExit = false)
            : base(id, useAsyncEnter, useAsyncExit)
        { }

        public BasicState AddOnEnterAsync(StateAsyncEvent onEnterAsync)
        {
            OnEnterAsync += onEnterAsync;
            return this;
        }
        public BasicState AddOnExitAsync(StateAsyncEvent onExitAsync)
        {
            OnExitAsync += onExitAsync;
            return this;
        }
        public BasicState AddOnEnter(StateEvent onEnter)
        {
            OnEnter += onEnter;
            return this;
        }
        public BasicState AddOnExit(StateEvent onExit)
        {
            OnExit += onExit;
            return this;
        }
        public BasicState AddOnTick(StateTick onTick)
        {
            OnTick += onTick;
            return this;
        }

        public sealed override IEnumerator EnterAsync(AbstractHPDAState currentState)
        {
            if (OnEnterAsync != null)
            {
                yield return CoroutineUtility.StartCoroutine(OnEnterAsync(currentState));
            }
        }
        public sealed override IEnumerator ExitAsync(AbstractHPDAState nextState)
        {
            if (OnExitAsync != null)
            {
                yield return CoroutineUtility.StartCoroutine(OnExitAsync(nextState));
            }
        }
        public sealed override void Enter(AbstractHPDAState previousState)
        {
            if (OnEnter != null)
            {
                OnEnter(previousState);
            }
        }
        public sealed override void Exit(AbstractHPDAState nextState)
        {
            if (OnExit != null)
            {
                OnExit(nextState);
            }
        }
        public sealed override void Tick()
        {
            if (OnTick != null)
            {
                OnTick();
            }
        }

        public sealed override string ToString()
        {
            return id;
        }
    }
}
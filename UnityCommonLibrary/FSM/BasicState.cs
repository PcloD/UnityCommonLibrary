using System.Collections;
using UnityCommonLibrary.Utilities;

namespace UnityCommonLibrary.FSM
{
    public sealed class BasicState : AbstractHPDAState
    {
        public delegate IEnumerator OnStateAsyncEvent(AbstractHPDAState otherState);
        public delegate void OnStateEvent(AbstractHPDAState otherState);
        public delegate void OnStateTick();

        private OnStateAsyncEvent onEnterAsync;
        private OnStateAsyncEvent onExitAsync;
        private OnStateEvent onEnter;
        private OnStateEvent onExit;
        private OnStateTick onTick;

        public BasicState(string id = null, bool useAsyncEnter = false, bool useAsyncExit = false)
            : base(id, useAsyncEnter, useAsyncExit)
        { }

        public BasicState AddOnEnterAsync(OnStateAsyncEvent onEnterAsync)
        {
            this.onEnterAsync += onEnterAsync;
            return this;
        }
        public BasicState AddOnExitAsync(OnStateAsyncEvent onExitAsync)
        {
            this.onExitAsync += onExitAsync;
            return this;
        }
        public BasicState AddOnEnter(OnStateEvent onEnter)
        {
            this.onEnter += onEnter;
            return this;
        }
        public BasicState AddOnExit(OnStateEvent onExit)
        {
            this.onExit += onExit;
            return this;
        }
        public BasicState AddOnTick(OnStateTick onTick)
        {
            this.onTick += onTick;
            return this;
        }

        public sealed override IEnumerator EnterAsync(AbstractHPDAState currentState)
        {
            if (onEnterAsync != null)
            {
                yield return CoroutineUtility.StartCoroutine(onEnterAsync(currentState));
            }
        }
        public sealed override IEnumerator ExitAsync(AbstractHPDAState nextState)
        {
            if (onExitAsync != null)
            {
                yield return CoroutineUtility.StartCoroutine(onExitAsync(nextState));
            }
        }
        public sealed override void Enter(AbstractHPDAState previousState)
        {
            if (onEnter != null)
            {
                onEnter(previousState);
            }
        }
        public sealed override void Exit(AbstractHPDAState nextState)
        {
            if (onExit != null)
            {
                onExit(nextState);
            }
        }
        public sealed override void Tick()
        {
            if (onTick != null)
            {
                onTick();
            }
        }

        public sealed override string ToString()
        {
            return id;
        }
    }
}
using System.Collections;

namespace UnityCommonLibrary.FSM
{
    public sealed class BasicState : AbstractHPDAState
    {
        public delegate void OnStateEvent(AbstractHPDAState state);
        public delegate void OnStateTick();

        private OnStateEvent onEnter;
        private OnStateEvent onExit;
        private OnStateTick onTick;

        public BasicState(string id = null) : base(id) { }

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

        public sealed override IEnumerator Enter(AbstractHPDAState currentState)
        {
            if (onEnter != null)
            {
                onEnter(currentState);
            }
            yield break;
        }
        public sealed override IEnumerator Exit(AbstractHPDAState nextState)
        {
            if (onExit != null)
            {
                onExit(nextState);
            }
            yield break;
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

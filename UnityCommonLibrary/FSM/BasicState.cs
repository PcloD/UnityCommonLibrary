using System.Collections;

namespace UnityCommonLibrary.FSM
{
    public sealed class BasicState : AbstractHPDAState
    {
        public delegate void OnStateAction();

        public readonly OnStateAction onEnter;
        public readonly OnStateAction onExit;
        public readonly OnStateAction onTick;

        public BasicState(string id = null, OnStateAction onEnter = null, OnStateAction onExit = null, OnStateAction onTick = null) : base(id)
        {
            this.onEnter = onEnter;
            this.onExit = onExit;
            this.onTick = onTick;
        }

        public override IEnumerator Enter()
        {
            if (onEnter != null)
            {
                onEnter();
            }
            yield break;
        }
        public override IEnumerator Exit()
        {
            if (onExit != null)
            {
                onExit();
            }
            yield break;
        }
        public override void Tick()
        {
            if (onTick != null)
            {
                onTick();
            }
        }
    }
}

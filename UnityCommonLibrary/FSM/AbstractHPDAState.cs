using System.Collections;
using UnityCommonLibrary.Time;
using UnityEngine;

namespace UnityCommonLibrary.FSM
{
    /// <summary>
    /// Represents a single state in a <see cref="HPDAStateMachine"/>
    /// </summary>
    public abstract class AbstractHPDAState
    {
        private readonly int hashcode;

        public bool useAsyncEnter;
        public bool useAsyncExit;
        public string id { get; private set; }
        public TimeSlice timeEntered { get; internal set; }

        public AbstractHPDAState(string id = null, bool useAsyncEnter = false, bool useAsyncExit = false)
        {
            this.id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;
            hashcode = Animator.StringToHash(this.id);
            this.useAsyncEnter = useAsyncEnter;
            this.useAsyncExit = useAsyncExit;
        }

        public virtual IEnumerator EnterAsync(AbstractHPDAState previousState) { yield break; }
        public virtual IEnumerator ExitAsync(AbstractHPDAState nextState) { yield break; }
        public virtual void Enter(AbstractHPDAState previousState) { }
        public virtual void Exit(AbstractHPDAState nextState) { }
        public virtual void Tick() { }

        public sealed override int GetHashCode()
        {
            return hashcode;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var other = obj as AbstractHPDAState;
            if (other == null)
            {
                return false;
            }
            return other.hashcode == hashcode;
        }
    }
}
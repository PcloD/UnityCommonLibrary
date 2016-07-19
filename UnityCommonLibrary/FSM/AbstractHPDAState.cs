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

        public string id { get; private set; }
        public TimeSlice timeEntered { get; internal set; }

        public AbstractHPDAState(string id = null)
        {
            this.id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;
            hashcode = Animator.StringToHash(this.id);
        }

        public abstract IEnumerator Enter(AbstractHPDAState previousState);
        public abstract IEnumerator Exit(AbstractHPDAState nextState);
        public abstract void Tick();

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
            return other.id == id;
        }
    }
}

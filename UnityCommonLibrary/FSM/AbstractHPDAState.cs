using System.Collections;
using UnityCommonLibrary.Time;

namespace UnityCommonLibrary.FSM
{
    /// <summary>
    /// Represents a single state in a <see cref="HPDAStateMachine"/>
    /// </summary>
    public abstract class AbstractHPDAState
    {
        public string id { get; private set; }
        public TimeSlice timeEntered { get; internal set; }

        public AbstractHPDAState(string id = null)
        {
            this.id = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString() : id;
        }

        public abstract IEnumerator Enter();
        public abstract IEnumerator Exit();
        public abstract void Tick();
    }
}

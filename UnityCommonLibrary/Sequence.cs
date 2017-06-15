using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    /// <summary>
    ///     Represents a series of Coroutines for creating
    ///     defined sequences of events, like an in-game cutscene or
    ///     tutorial.
    /// </summary>
    public abstract class Sequence : MonoBehaviour
    {
        /// <summary>
        ///     Represents the current TimerState of the sequence.
        /// </summary>
        public enum Status
        {
            Inactive,
            Active,
            Paused
        }

        /// <summary>
        ///     Should sequence MachineStatus changes be logged?
        /// </summary>
        public static bool LogSequenceEvents;

        /// <summary>
        ///     Every existing sequence, for running tasks on all sequences.
        /// </summary>
        private static readonly List<Sequence> _sequences = new List<Sequence>();

        /// <summary>
        ///     Should the sequence object be destroyed when complete?
        /// </summary>
        [HideInInspector]
        public bool DestroyOnComplete;

        /// <summary>
        ///     If true, call <see cref="Execute" /> when <see cref="Start" /> is called
        /// </summary>
        [HideInInspector]
        public bool ExecuteOnStart;

        /// <summary>
        ///     Should the sequence loop when complete?
        /// </summary>
        [HideInInspector]
        public bool Loop;

        /// <summary>
        ///     The representation of our running routine.
        /// </summary>
        private Coroutine _routineExecutor;

        private Queue<IEnumerator> _routines = new Queue<IEnumerator>();

        /// <summary>
        ///     The representation of our running sequence.
        /// </summary>
        private Coroutine _sequenceExecutor;

        // Static events fired for every sequence
        public delegate void OnAnySequenceEvent(Sequence s);

        // Sequence instance specific events
        public delegate void OnSequenceEvent();

        public static event OnAnySequenceEvent AnySequenceComplete;
        public static event OnAnySequenceEvent AnySequenceHalted;
        public static event OnAnySequenceEvent AnySequencePaused;
        public static event OnAnySequenceEvent AnySequenceResumed;
        public static event OnAnySequenceEvent AnySequenceStarted;
        public event OnSequenceEvent RoutineComplete;
        public event OnSequenceEvent RoutineStarted;
        public event OnSequenceEvent SequenceComplete;
        public event OnSequenceEvent SequenceHalted;
        public event OnSequenceEvent SequencePaused;
        public event OnSequenceEvent SequenceResumed;
        public event OnSequenceEvent SequenceStarted;

        public bool IsRunningRoutine
        {
            get { return _routineExecutor != null; }
        }

        public int RoutinesLeft
        {
            get { return _routines.Count; }
        }

        /// <summary>
        ///     The current playback MachineStatus of this sequence.
        /// </summary>
        public Status SequenceStatus { get; private set; }

        public int TotalRoutineCount { get; private set; }

        public static void PauseAll()
        {
            foreach (var s in _sequences)
            {
                s.Pause();
            }
        }

        public static void ResumeAll()
        {
            foreach (var s in _sequences)
            {
                s.Resume();
            }
        }

        public static void ExecuteAll()
        {
            foreach (var s in _sequences)
            {
                s.Execute();
            }
        }

        public static void HaltAll()
        {
            foreach (var s in _sequences)
            {
                s.Halt();
            }
        }

        /// <summary>
        ///     Creates a new instance of the Sequence of StateType <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The StateType of sequence to create.</typeparam>
        /// <param name="destroyOnComplete">
        ///     Sets the sequence's destroyOnComplete flag, defaults to true. Also destroys on halt if
        ///     true.
        /// </param>
        /// <returns>The new Sequence instance.</returns>
        public static T Create<T>(bool destroyOnComplete = true) where T : Sequence
        {
            var parent = new GameObject(typeof(T).Name);
            var me = parent.AddComponent<T>();
            me.DestroyOnComplete = destroyOnComplete;
            if (destroyOnComplete)
            {
                me.SequenceHalted += () => { Destroy(parent); };
            }
            return me;
        }

        /// <summary>
        ///     Resets and begins playback.
        /// </summary>
        public void Execute()
        {
            if (SequenceStatus != Status.Inactive)
            {
                return;
            }
            ResetSequence();
            SequenceStatus = Status.Active;
            if (SequenceStarted != null)
            {
                SequenceStarted();
            }
            if (AnySequenceStarted != null)
            {
                AnySequenceStarted(this);
            }
            StartCoroutine(ExecuteSequence());
        }

        /// <summary>
        ///     Stops and resets this sequence.
        /// </summary>
        /// <param name="obeyLoopFlag">Should the <see cref="Loop" /> flag be ignored for this halt?</param>
        public void Halt(bool obeyLoopFlag = false)
        {
            if (SequenceStatus != Status.Active)
            {
                return;
            }

            StopAllCoroutines();
            ResetSequence();

            SequenceStatus = Status.Inactive;

            if (SequenceHalted != null)
            {
                SequenceHalted();
            }
            if (AnySequenceHalted != null)
            {
                AnySequenceHalted(this);
            }
            if (Loop && obeyLoopFlag)
            {
                Execute();
            }
        }

        /// <summary>
        ///     Pauses playback (only if currently playing)
        /// </summary>
        public void Pause()
        {
            if (SequenceStatus != Status.Active)
            {
                return;
            }

            SequenceStatus = Status.Paused;

            if (SequencePaused != null)
            {
                SequencePaused();
            }
            if (AnySequencePaused != null)
            {
                AnySequencePaused(this);
            }
        }

        /// <summary>
        ///     Resumes from paused TimerState.
        /// </summary>
        public void Resume()
        {
            if (SequenceStatus != Status.Paused)
            {
                return;
            }

            SequenceStatus = Status.Active;

            if (SequenceResumed != null)
            {
                SequenceResumed();
            }
            if (AnySequenceResumed != null)
            {
                AnySequenceResumed(this);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} in task {1}", GetType().Name,
                TotalRoutineCount - RoutinesLeft);
        }

        /// <summary>
        ///     Introduces a delay via <see cref="UnityEngine.Time.unscaledTime" />
        /// </summary>
        /// <param name="time">How long to delay before continuing.</param>
        protected internal IEnumerator Delay(float time)
        {
            var start = UnityEngine.Time.unscaledTime;
            while (UnityEngine.Time.unscaledTime - start < time)
            {
                yield return null;
            }
        }

        /// <summary>
        ///     Collects all coroutines for execution.
        /// </summary>
        protected internal abstract IEnumerator[] Initialize();

        /// <summary>
        ///     Invokes a method without blocking.
        /// </summary>
        /// <param name="a">The callback to invoke.</param>
        protected internal IEnumerator MainThread(Action a)
        {
            a.Invoke();
            yield break;
        }

        /// <summary>
        ///     Invokes a Coroutine without blocking.
        /// </summary>
        /// <param name="a">The coroutine to start.</param>
        protected internal IEnumerator NonBlock(IEnumerator ienum)
        {
            StartCoroutine(ienum);
            yield break;
        }

        /// <summary>
        ///     Executes standard <see cref="WaitForSeconds" />
        /// </summary>
        /// <param name="time">How long to delay before continuing.</param>
        protected internal IEnumerator ScaledDelay(float time)
        {
            yield return new WaitForSeconds(time);
        }

        protected virtual void Awake()
        {
            _sequences.Add(this);
        }

        protected virtual void OnDestroy()
        {
            _sequences.Remove(this);
        }

        protected virtual void Start()
        {
            if (ExecuteOnStart)
            {
                Execute();
            }
        }

        /// <summary>
        ///     Runs tasks related to sequence completion, including looping and destroying if needed.
        /// </summary>
        private void CompleteSequence()
        {
            ResetSequence();

            SequenceStatus = Status.Inactive;

            if (SequenceComplete != null)
            {
                SequenceComplete();
            }
            if (AnySequenceComplete != null)
            {
                AnySequenceComplete(this);
            }
            if (DestroyOnComplete)
            {
                Destroy(this);
            }
            else if (Loop)
            {
                Execute();
            }
        }

        /// <summary>
        ///     A custom Coroutine executor to add callback functionality.
        /// </summary>
        /// <param name="routine">The coroutine to execute</param>
        /// <returns></returns>
        private IEnumerator ExecuteRoutine(IEnumerator routine)
        {
            while (true)
            {
                var hasInstruction = routine.MoveNext();
                if (SequenceStatus == Status.Inactive || !hasInstruction)
                {
                    break;
                }
                yield return routine.Current;
            }
            _routineExecutor = null;
            if (RoutineComplete != null)
            {
                RoutineComplete();
            }
        }

        /// <summary>
        ///     Executes every routine in this Sequence.
        /// </summary>
        private IEnumerator ExecuteSequence()
        {
            while (true)
            {
                // Exit if not running a task and there are none left
                if (RoutinesLeft == 0 && !IsRunningRoutine)
                {
                    break;
                }
                // Start next routine only if we're not paused
                if (SequenceStatus == Status.Active && !IsRunningRoutine)
                {
                    // Fire callback for new routine
                    if (RoutineStarted != null)
                    {
                        RoutineStarted();
                    }
                    if (LogSequenceEvents)
                    {
                        print(ToString());
                    }
                    var routine = _routines.Dequeue();
                    _routineExecutor = StartCoroutine(ExecuteRoutine(routine));
                }
                yield return null;
            }
            CompleteSequence();
        }

        /// <summary>
        ///     Stops any executors and reloads all routines into queue.
        /// </summary>
        private void ResetSequence()
        {
            if (_routineExecutor != null)
            {
                StopCoroutine(_routineExecutor);
            }
            if (_sequenceExecutor != null)
            {
                StopCoroutine(_sequenceExecutor);
            }
            // Reload queue
            _routines = new Queue<IEnumerator>(Initialize());
            TotalRoutineCount = _routines.Count;
        }
    }
}
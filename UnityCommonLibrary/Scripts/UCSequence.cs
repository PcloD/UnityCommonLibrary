using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary {
    public abstract class UCSequence : UCScript {
        static List<UCSequence> sequences = new List<UCSequence>();

        #region Events & Delegates
        public delegate void OnAnyEventStarted(UCSequence ucs);
        public static event OnAnyEventStarted AnyEventStarted;

        public delegate void OnAnyEventPaused(UCSequence ucs);
        public static event OnAnyEventPaused AnyEventPaused;

        public delegate void OnAnyEventResumed(UCSequence ucs);
        public static event OnAnyEventResumed AnyEventResumed;

        public delegate void OnAnyEventHalted(UCSequence ucs);
        public static event OnAnyEventHalted AnyEventHalted;

        public delegate void OnAnyEventCompleted(UCSequence ucs);
        public static event OnAnyEventCompleted AnyEventCompleted;

        public delegate void OnEventStarted();
        public event OnEventStarted EventStarted;

        public delegate void OnEventPaused();
        public event OnEventPaused EventPaused;

        public delegate void OnEventResumed();
        public event OnEventResumed EventResumed;

        public delegate void OnEventHalted();
        public event OnEventHalted EventHalted;

        public delegate void OnEventCompleted();
        public event OnEventCompleted EventCompleted;

        public delegate void OnTaskStarted();
        public event OnTaskStarted TaskStarted;

        public delegate void OnTaskCompleted();
        public event OnTaskCompleted TaskCompleted;
        #endregion

        public bool executeOnStart, loop, destroyOnComplete;

        public static bool logTaskEvents;

        public EventStatus status { get; private set; }
        public bool isRunningTask { get { return taskRoutine != null; } }
        public int totalTaskCount { get; private set; }
        public int tasksLeft { get { return tasks.Count; } }

        Queue<IEnumerator> tasks = new Queue<IEnumerator>();
        Coroutine sequenceRoutine;
        Coroutine taskRoutine;

        void Awake() {
            sequences.Add(this);
        }

        void Start() {
            if(executeOnStart) {
                Execute();
            }
        }

        IEnumerator ExecuteSequence() {
            while(true) {
                if(tasksLeft == 0 && !isRunningTask) {
                    break;
                }
                else if(status == EventStatus.Active && !isRunningTask) {
                    if(TaskStarted != null) {
                        TaskStarted();
                    }
                    if(logTaskEvents) {
                        print(ToString());
                    }
                    var task = tasks.Dequeue();
                    taskRoutine = StartCoroutine(ExecSequenceTask(task));
                }
                yield return null;
            }
            CompleteEvent();
        }

        private IEnumerator ExecSequenceTask(IEnumerator task) {
            while(true) {
                var hasInstruction = task.MoveNext();

                if(status == EventStatus.Inactive || !hasInstruction) {
                    break;
                }
                yield return task.Current;
            }
            taskRoutine = null;
            if(TaskCompleted != null) {
                TaskCompleted();
            }
        }

        void OnDestroy() {
            sequences.Remove(this);
        }

        void ResetEvent() {
            if(taskRoutine != null) {
                StopCoroutine(taskRoutine);
            }
            if(sequenceRoutine != null) {
                StopCoroutine(sequenceRoutine);
            }
            tasks = new Queue<IEnumerator>(Initialize());
            totalTaskCount = tasks.Count;
        }

        void CompleteEvent() {
            ResetEvent();

            status = EventStatus.Inactive;

            if(EventCompleted != null) {
                EventCompleted();
            }
            if(AnyEventCompleted != null) {
                AnyEventCompleted(this);
            }
            if(destroyOnComplete) {
                Destroy(this);
            }
            else if(loop) {
                Execute();
            }
        }

        #region Controls
        public void Execute() {
            if(status != EventStatus.Inactive) {
                return;
            }
            ResetEvent();
            status = EventStatus.Active;
            if(EventStarted != null) {
                EventStarted();
            }
            if(AnyEventStarted != null) {
                AnyEventStarted(this);
            }
            StartCoroutine(ExecuteSequence());
        }

        public void Pause() {
            if(status != EventStatus.Active) {
                return;
            }

            status = EventStatus.Paused;

            if(EventPaused != null) {
                EventPaused();
            }
            if(AnyEventPaused != null) {
                AnyEventPaused(this);
            }
        }

        public void Resume() {
            if(status != EventStatus.Paused) {
                return;
            }

            status = EventStatus.Active;

            if(EventResumed != null) {
                EventResumed();
            }
            if(AnyEventResumed != null) {
                AnyEventResumed(this);
            }
        }

        public void Halt(bool obeyLoop = false) {
            if(status != EventStatus.Active) {
                return;
            }

            StopAllCoroutines();
            ResetEvent();

            status = EventStatus.Inactive;

            if(EventHalted != null) {
                EventHalted();
            }
            if(AnyEventHalted != null) {
                AnyEventHalted(this);
            }
            if(loop && obeyLoop) {
                Execute();
            }
        }
        #endregion

        #region Utility/Batch Methods
        public static void PauseAll() {
            foreach(var s in sequences) {
                s.Pause();
            }
        }

        public static void ResumeAll() {
            foreach(var s in sequences) {
                s.Resume();
            }
        }

        public static void ExecuteAll() {
            foreach(var s in sequences) {
                s.Execute();
            }
        }

        public static void HaltAll() {
            foreach(var s in sequences) {
                s.Halt();
            }
        }
        #endregion

        /// <summary>
        /// Passes all of the coroutines to the base class for execution.
        /// </summary>
        /// <returns></returns>
        protected internal abstract IEnumerator[] Initialize();

        /// <summary>
        /// Provided for easy delays between events.
        /// </summary>
        /// <param name="time">How long to delay before continuing</param>
        /// <returns></returns>
        protected internal IEnumerator Delay(float time) {
            var start = Time.unscaledTime;
            while(Time.unscaledTime - start < time) {
                yield return null;
            }
        }

        protected internal IEnumerator ScaledDelay(float time) {
            yield return new WaitForSeconds(time);
        }

        protected internal IEnumerator MainThread(Action a) {
            a.Invoke();
            yield return null;
        }

        protected internal IEnumerator NonBlock(IEnumerator ienum) {
            StartCoroutine(ienum);
            yield return null;
        }

        public static T Create<T>(bool destroyOnComplete = true) where T : UCSequence {
            var parent = new GameObject(typeof(T).Name);
            var me = parent.AddComponent<T>();
            me.destroyOnComplete = destroyOnComplete;
            if(destroyOnComplete) {
                me.EventHalted += () => {
                    Destroy(parent);
                };
            }
            return me;
        }

        public enum EventStatus {
            Inactive,
            Active,
            Paused
        }

        public override string ToString() {
            return string.Format("{0} in task {1}", GetType().Name, totalTaskCount - tasksLeft);
        }

    }
}
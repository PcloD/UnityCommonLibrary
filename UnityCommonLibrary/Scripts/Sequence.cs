using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
	/// <summary>
	/// Represents a series of Coroutines for creating
	/// defined sequences of events, like an in-game cutscene or
	/// tutorial.
	/// </summary>
	public abstract class Sequence : MonoBehaviour
	{
		#region Events & Delegates
		// Static events fired for every sequence
		public delegate void OnAnySequenceEvent(Sequence s);
		public static event OnAnySequenceEvent AnySequenceStarted;
		public static event OnAnySequenceEvent AnySequencePaused;
		public static event OnAnySequenceEvent AnySequenceResumed;
		public static event OnAnySequenceEvent AnySequenceHalted;
		public static event OnAnySequenceEvent AnySequenceComplete;

		// Sequence instance specific events
		public delegate void OnSequenceEvent();
		public event OnSequenceEvent SequenceStarted;
		public event OnSequenceEvent SequencePaused;
		public event OnSequenceEvent SequenceResumed;
		public event OnSequenceEvent SequenceHalted;
		public event OnSequenceEvent SequenceComplete;
		public event OnSequenceEvent RoutineStarted;
		public event OnSequenceEvent RoutineComplete;
		#endregion

		#region Static Fields
		/// <summary>
		/// Every existing sequence, for running tasks on all sequences.
		/// </summary>
		private static readonly List<Sequence> sequences = new List<Sequence>();
		/// <summary>
		/// Should sequence status changes be logged?
		/// </summary>
		public static bool logSequenceEvents;
		#endregion

		#region Public Fields
		/// <summary>
		/// If true, call <see cref="Execute"/> when <see cref="Start"/> is called
		/// </summary>
		[HideInInspector]
		public bool executeOnStart;
		/// <summary>
		/// Should the sequence loop when complete?
		/// </summary>
		[HideInInspector]
		public bool loop;
		/// <summary>
		/// Should the sequence object be destroyed when complete?
		/// </summary>
		[HideInInspector]
		public bool destroyOnComplete;
		#endregion

		#region Properties
		/// <summary>
		/// The current playback status of this sequence.
		/// </summary>
		public Status status { get; private set; }
		public bool isRunningRoutine
		{
			get
			{
				return routineExecutor != null;
			}
		}
		public int totalRoutineCount { get; private set; }
		public int routinesLeft
		{
			get
			{
				return routines.Count;
			}
		}
		#endregion

		#region Private Fields
		private Queue<IEnumerator> routines = new Queue<IEnumerator>();
		/// <summary>
		/// The representation of our running sequence.
		/// </summary>
		private Coroutine sequenceExecutor;
		/// <summary>
		/// The representation of our running routine.
		/// </summary>
		private Coroutine routineExecutor;
		#endregion

		#region Unity Messages
		protected virtual void Awake()
		{
			sequences.Add(this);
		}
		protected virtual void Start()
		{
			if(executeOnStart)
			{
				Execute();
			}
		}
		protected virtual void OnDestroy()
		{
			sequences.Remove(this);
		}
		#endregion

		#region Execution Tasks
		/// <summary>
		/// Executes every routine in this Sequence.
		/// </summary>
		private IEnumerator ExecuteSequence()
		{
			while(true)
			{
				// Exit if not running a task and there are none left
				if(routinesLeft == 0 && !isRunningRoutine)
				{
					break;
				}
				// Start next routine only if we're not paused
				else if(status == Status.Active && !isRunningRoutine)
				{
					// Fire callback for new routine
					if(RoutineStarted != null)
					{
						RoutineStarted();
					}
					if(logSequenceEvents)
					{
						print(ToString());
					}
					var routine = routines.Dequeue();
					routineExecutor = StartCoroutine(ExecuteRoutine(routine));
				}
				yield return null;
			}
			CompleteSequence();
		}
		/// <summary>
		/// A custom Coroutine executor to add callback functionality.
		/// </summary>
		/// <param name="routine">The coroutine to execute</param>
		/// <returns></returns>
		private IEnumerator ExecuteRoutine(IEnumerator routine)
		{
			while(true)
			{
				var hasInstruction = routine.MoveNext();
				if(status == Status.Inactive || !hasInstruction)
				{
					break;
				}
				yield return routine.Current;
			}
			routineExecutor = null;
			if(RoutineComplete != null)
			{
				RoutineComplete();
			}
		}
		/// <summary>
		/// Collects all coroutines for execution.
		/// </summary>
		protected internal abstract IEnumerator[] Initialize();
		#endregion

		#region Playback
		/// <summary>
		/// Resets and begins playback.
		/// </summary>
		public void Execute()
		{
			if(status != Status.Inactive)
			{
				return;
			}
			ResetSequence();
			status = Status.Active;
			if(SequenceStarted != null)
			{
				SequenceStarted();
			}
			if(AnySequenceStarted != null)
			{
				AnySequenceStarted(this);
			}
			StartCoroutine(ExecuteSequence());
		}
		/// <summary>
		/// Pauses playback (only if currently playing)
		/// </summary>
		public void Pause()
		{
			if(status != Status.Active)
			{
				return;
			}

			status = Status.Paused;

			if(SequencePaused != null)
			{
				SequencePaused();
			}
			if(AnySequencePaused != null)
			{
				AnySequencePaused(this);
			}
		}
		/// <summary>
		/// Resumes from paused state.
		/// </summary>
		public void Resume()
		{
			if(status != Status.Paused)
			{
				return;
			}

			status = Status.Active;

			if(SequenceResumed != null)
			{
				SequenceResumed();
			}
			if(AnySequenceResumed != null)
			{
				AnySequenceResumed(this);
			}
		}
		/// <summary>
		/// Stops and resets this sequence.
		/// </summary>
		/// <param name="obeyLoopFlag">Should the <see cref="loop"/> flag be ignored for this halt?</param>
		public void Halt(bool obeyLoopFlag = false)
		{
			if(status != Status.Active)
			{
				return;
			}

			StopAllCoroutines();
			ResetSequence();

			status = Status.Inactive;

			if(SequenceHalted != null)
			{
				SequenceHalted();
			}
			if(AnySequenceHalted != null)
			{
				AnySequenceHalted(this);
			}
			if(loop && obeyLoopFlag)
			{
				Execute();
			}
		}
		/// <summary>
		/// Stops any executors and reloads all routines into queue.
		/// </summary>
		private void ResetSequence()
		{
			if(routineExecutor != null)
			{
				StopCoroutine(routineExecutor);
			}
			if(sequenceExecutor != null)
			{
				StopCoroutine(sequenceExecutor);
			}
			// Reload queue
			routines = new Queue<IEnumerator>(Initialize());
			totalRoutineCount = routines.Count;
		}
		/// <summary>
		/// Runs tasks related to sequence completion, including looping and destroying if needed.
		/// </summary>
		private void CompleteSequence()
		{
			ResetSequence();

			status = Status.Inactive;

			if(SequenceComplete != null)
			{
				SequenceComplete();
			}
			if(AnySequenceComplete != null)
			{
				AnySequenceComplete(this);
			}
			if(destroyOnComplete)
			{
				Destroy(this);
			}
			else if(loop)
			{
				Execute();
			}
		}
		#endregion

		#region Utility/Batch Methods
		public static void PauseAll()
		{
			foreach(var s in sequences)
			{
				s.Pause();
			}
		}
		public static void ResumeAll()
		{
			foreach(var s in sequences)
			{
				s.Resume();
			}
		}
		public static void ExecuteAll()
		{
			foreach(var s in sequences)
			{
				s.Execute();
			}
		}
		public static void HaltAll()
		{
			foreach(var s in sequences)
			{
				s.Halt();
			}
		}
		/// <summary>
		/// Creates a new instance of the Sequence of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of sequence to create.</typeparam>
		/// <param name="destroyOnComplete">Sets the sequence's destroyOnComplete flag, defaults to true. Also destroys on halt if true.</param>
		/// <returns>The new Sequence instance.</returns>
		public static T Create<T>(bool destroyOnComplete = true) where T : Sequence
		{
			var parent = new GameObject(typeof(T).Name);
			var me = parent.AddComponent<T>();
			me.destroyOnComplete = destroyOnComplete;
			if(destroyOnComplete)
			{
				me.SequenceHalted += () =>
				{
					Destroy(parent);
				};
			}
			return me;
		}
		#endregion

		#region Sequence Tasks
		/// <summary>
		/// Introduces a delay via <see cref="UnityEngine.Time.unscaledTime"/>
		/// </summary>
		/// <param name="time">How long to delay before continuing.</param>
		protected internal IEnumerator Delay(float time)
		{
			var start = UnityEngine.Time.unscaledTime;
			while(UnityEngine.Time.unscaledTime - start < time)
			{
				yield return null;
			}
		}
		/// <summary>
		/// Executes standard <see cref="WaitForSeconds"/>
		/// </summary>
		/// <param name="time">How long to delay before continuing.</param>
		protected internal IEnumerator ScaledDelay(float time)
		{
			yield return new WaitForSeconds(time);
		}
		/// <summary>
		/// Invokes a method without blocking.
		/// </summary>
		/// <param name="a">The callback to invoke.</param>
		protected internal IEnumerator MainThread(Action a)
		{
			a.Invoke();
			yield break;
		}
		/// <summary>
		/// Invokes a Coroutine without blocking.
		/// </summary>
		/// <param name="a">The coroutine to start.</param>
		protected internal IEnumerator NonBlock(IEnumerator ienum)
		{
			StartCoroutine(ienum);
			yield break;
		}
		#endregion

		public override string ToString()
		{
			return string.Format("{0} in task {1}", GetType().Name, totalRoutineCount - routinesLeft);
		}

		/// <summary>
		/// Represents the current state of the sequence.
		/// </summary>
		public enum Status
		{
			Inactive,
			Active,
			Paused
		}
	}
}
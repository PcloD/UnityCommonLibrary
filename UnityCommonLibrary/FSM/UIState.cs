using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary.FSM {
	/// <summary>
	/// A <see cref="HPDAState"/> that inheritly has a CanvasGroup.
	/// Removes need for boilerplate code when creating states for a 
	/// menu system that incorprates a <see cref="HPDAStateMachine"/>
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class UIState : HPDAState {
		/// <summary>
		/// CanvasGroup used for disabling visuals and interaction
		/// when this state is inactive.
		/// </summary>
		private CanvasGroup _canvasGroup;
		protected CanvasGroup canvasGroup {
			get {
				if(_canvasGroup == null) {
					_canvasGroup = GetComponent<CanvasGroup>();
				}
				return _canvasGroup;
			}
		}

		/// <summary>
		/// Makes state totally functionally inactive.
		/// Deactivates GameObject to make UI navigation
		/// work on only active state.s
		/// </summary>
		protected virtual void Start() {
			canvasGroup.blocksRaycasts = false;
			canvasGroup.alpha = 0f;
			gameObject.SetActive(false);
		}

		public override IEnumerator Enter() {
			gameObject.SetActive(true);
			yield break;
		}

		public override IEnumerator Exit() {
			gameObject.SetActive(false);
			yield break;
		}
	}
}

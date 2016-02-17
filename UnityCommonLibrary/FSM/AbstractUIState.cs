using UnityEngine;

namespace UnityCommonLibrary.FSM {
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AbstractUIState : AbstractFSMState {
        protected CanvasGroup canvasGroup { get; private set; }

        protected void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            ResetState();
        }

        public override void ResetState() {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}

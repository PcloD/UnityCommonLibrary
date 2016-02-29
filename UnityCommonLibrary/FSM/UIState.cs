using UnityEngine;

namespace UnityCommonLibrary.FSM {
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIState : HPDAState {
        private CanvasGroup _canvasGroup;
        protected CanvasGroup canvasGroup {
            get {
                if(_canvasGroup == null) {
                    _canvasGroup = GetComponent<CanvasGroup>();
                }
                return _canvasGroup;
            }
        }

        public override void Initialize() {
            canvasGroup.blocksRaycasts = false;
        }
    }
}

using System.Collections;
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

        protected virtual void Start() {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public override IEnumerator Enter() {
            gameObject.SetActive(true);
            return base.Enter();
        }

        public override IEnumerator Exit() {
            gameObject.SetActive(false);
            return base.Enter();
        }
    }
}

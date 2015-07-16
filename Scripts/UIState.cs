using UnityEngine;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIState : GameState {

        protected CanvasGroup group { get; private set; }

        [SerializeField]
        GameObject _firstSelected;

        public GameObject firstSelected { get { return _firstSelected; } }

        protected override void Awake() {
            base.Awake();
            group = GetComponent<CanvasGroup>();
            group.alpha = 0f;
            group.blocksRaycasts = false;
        }
    }
}
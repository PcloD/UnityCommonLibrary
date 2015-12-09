using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    [RequireComponent(typeof(Selectable))]
    public class LabeledGraphic : MonoBehaviour {
        [SerializeField]
        Text _label;

        public Selectable graphic { get; private set; }
        public Text label { get { return _label; } }

        void Awake() {
            graphic = GetComponent<Selectable>();
        }
    }
}
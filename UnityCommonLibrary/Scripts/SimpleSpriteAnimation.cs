using UnityCommonLibrary.Time;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary {
    public class SimpleSpriteAnimation : MonoBehaviour {
        public TimeMode timeMode;
        [SerializeField]
        private float _fps = 30f;
        public bool loop;
        public Sprite[] frames;

        public float fps {
            get {
                return _fps;
            }
            set {
                _fps = value;
                timer.duration = fps == 0f ? 0f : 1f / fps;
            }
        }
        public UTimer timer { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }
        public Image img { get; private set; }
        private int index;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            img = GetComponent<Image>();

            timer = new UTimer();
            timer.TimerElapsed += Timer_TimerElapsed;
            timer.Restart();
        }

        private void Timer_TimerElapsed() {
            if(!GotoIndex(index)) {
                if(loop) {
                    index = 0;
                }
                return;
            }
            index++;
            timer.Restart();
        }

        public void PlayOneShot() {
            enabled = true;
            index = 1;
            timer.Restart();
            GotoIndex(0);
            loop = false;
        }

        public bool GotoIndex(int index) {
            if(index >= frames.Length || index < 0) {
                return false;
            }
            if(spriteRenderer != null) {
                spriteRenderer.sprite = frames[index];
            }
            if(img != null) {
                img.sprite = frames[index];
            }
            this.index = index;
            return true;
        }
    }
}

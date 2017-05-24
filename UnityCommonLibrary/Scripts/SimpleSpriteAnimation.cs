using UnityCommonLibrary.Time;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary
{
    public class SimpleSpriteAnimation : MonoBehaviour
    {
        public Sprite[] Frames;
        public bool Loop;
        public TimeMode TimeMode;

        [SerializeField]
        private float _fps = 30f;

        private int _index;
        public Image Img { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }

        public UTimer Timer { get; } = new UTimer();

        public bool GotoIndex(int index)
        {
            if (index >= Frames.Length || index < 0)
            {
                return false;
            }
            if (SpriteRenderer != null)
            {
                SpriteRenderer.sprite = Frames[index];
            }
            if (Img != null)
            {
                Img.sprite = Frames[index];
            }
            _index = index;
            return true;
        }

        public void PlayOneShot()
        {
            enabled = true;
            _index = 1;
            Timer.Restart();
            GotoIndex(0);
            Loop = false;
        }

        public void SetDurationFromFps()
        {
            Timer.Duration = _fps == 0f ? 0f : 1f / _fps;
        }

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Img = GetComponent<Image>();

            SetDurationFromFps();
            Timer.TimerElapsed += Timer_TimerElapsed;
            Timer.Restart();
        }

        private void Timer_TimerElapsed()
        {
            if (!GotoIndex(_index))
            {
                if (Loop)
                {
                    _index = 0;
                }
                return;
            }
            SetDurationFromFps();
            _index++;
            Timer.Restart();
        }
    }
}
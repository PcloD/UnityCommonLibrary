using UnityCommonLibrary.Time;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCommonLibrary
{
	public class SimpleSpriteAnimation : MonoBehaviour
	{
		public TimeMode timeMode;
		[SerializeField]
		private float fps = 30f;
		public bool loop;
		public Sprite[] frames;

		public UTimer timer
		{
			get
			{
				return _timer;
			}
		}
		public SpriteRenderer spriteRenderer { get; private set; }
		public Image img { get; private set; }

		private UTimer _timer = new UTimer();
		private int index;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			img = GetComponent<Image>();

			SetDurationFromFPS();
			timer.TimerElapsed += Timer_TimerElapsed;
			timer.Restart();
		}

		public void SetDurationFromFPS()
		{
			timer.duration = fps == 0f ? 0f : 1f / fps;
		}

		private void Timer_TimerElapsed()
		{
			if(!GotoIndex(index))
			{
				if(loop)
				{
					index = 0;
				}
				return;
			}
			SetDurationFromFPS();
			index++;
			timer.Restart();
		}

		public void PlayOneShot()
		{
			enabled = true;
			index = 1;
			timer.Restart();
			GotoIndex(0);
			loop = false;
		}

		public bool GotoIndex(int index)
		{
			if(index >= frames.Length || index < 0)
			{
				return false;
			}
			if(spriteRenderer != null)
			{
				spriteRenderer.sprite = frames[index];
			}
			if(img != null)
			{
				img.sprite = frames[index];
			}
			this.index = index;
			return true;
		}
	}
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityCommonLibrary.UI
{
	public class Selector : Selectable, IPointerClickHandler
	{
		public bool wrapAround;
		public Text label;
		public int selected = 0;
		public List<string> options;

		public override Selectable FindSelectableOnLeft()
		{
			selected--;
			return this;
		}
		public override Selectable FindSelectableOnRight()
		{
			selected++;
			return this;
		}

		private void Update()
		{
			CheckSelected();
			SetLabel();
		}

		protected override void Start()
		{
			base.Start();
			SetLabel();
		}

		private void SetLabel()
		{
			if(label == null)
			{
				return;
			}
			if(options.Count > 0)
			{
				label.text = options[selected];
			}
			else
			{
				label.text = "NO OPTIONS";
			}
		}

		private void CheckSelected()
		{
			if(wrapAround)
			{
				if(selected >= options.Count)
				{
					selected = 0;
				}
				else if(selected < 0)
				{
					selected = options.Count - 1;
				}
			}
			else
			{
				selected = Mathf.Clamp(selected, 0, options.Count - 1);
			}
		}

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            Update();
        }

        protected override void Reset() {
            base.Reset();
            options = new List<string>(new string[] { "Option A", "Option B", "Option C" });
            label = GetComponentInChildren<Text>();
        }
#endif

		// Custom logic for mouse support
		public void OnPointerClick(PointerEventData eventData)
		{
			switch(eventData.button)
			{
				case PointerEventData.InputButton.Left:
					selected++;
					break;
				case PointerEventData.InputButton.Right:
					selected--;
					break;
			}
			if(selected >= options.Count)
			{
				selected = 0;
			}
			else if(selected < 0)
			{
				selected = options.Count - 1;
			}
		}
	}
}
using System;
using UnityEngine;

namespace UnityCommonLibrary
{
	[DisallowMultipleComponent]
	public abstract class MultiTagApplicator<T> : MonoBehaviour where T : struct, IFormattable, IConvertible, IComparable
	{
		public bool applyRecursively;
		public T tags;

		protected virtual void Awake()
		{
			this.AddTags(tags);
			if(applyRecursively)
			{
				AddTagsToChildren(transform);
			}
		}
		protected virtual void AddTagsToChildren(Transform transform)
		{
			for(int i = 0; i < transform.childCount; i++)
			{
				var child = transform.GetChild(i);
				AddTagsToChildren(transform.GetChild(i));
			}
			transform.AddTags(tags);
		}
	}
}
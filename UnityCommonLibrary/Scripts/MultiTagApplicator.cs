using System;
using UnityEngine;

namespace UnityCommonLibrary
{
    [DisallowMultipleComponent]
    public abstract class MultiTagApplicator<T> : MonoBehaviour
        where T : struct, IFormattable, IConvertible, IComparable
    {
        public bool ApplyRecursively;
        public T Tags;

        protected virtual void AddTagsToChildren(Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                AddTagsToChildren(child);
            }
            transform.AddTags(Tags);
        }

        protected virtual void Awake()
        {
            this.AddTags(Tags);
            if (ApplyRecursively)
            {
                AddTagsToChildren(transform);
            }
        }
    }
}
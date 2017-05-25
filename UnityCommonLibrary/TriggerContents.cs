using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    /// <summary>
    ///     TODO: Optimize to use C# events
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerContents : MonoBehaviour
    {
        private readonly HashSet<Collider> _contents = new HashSet<Collider>();

        public delegate void OnContentsChanged(bool major);

        public event OnContentsChanged ContentsChanged;
        public Collider Collider { get; private set; }

        public HashSet<Collider> Contents
        {
            get
            {
                // Remove all null entries
                _contents.RemoveWhere(c => !c);
                return _contents;
            }
        }

        public bool HasAny
        {
            get { return _contents.Count > 0; }
        }

        private void Awake()
        {
            Collider = GetComponent<Collider>();
            if (!Collider.isTrigger)
            {
                UCLCore.Logger.LogError("", "COLLIDER MUST BE TRIGGER!");
            }
        }

        private void OnTriggerEnter(Collider c)
        {
            _contents.Add(c);
            if (ContentsChanged != null)
            {
                ContentsChanged(_contents.Count == 1);
            }
        }

        private void OnTriggerExit(Collider c)
        {
            _contents.Remove(c);
            if (ContentsChanged != null)
            {
                ContentsChanged(_contents.Count == 0);
            }
        }
    }
}
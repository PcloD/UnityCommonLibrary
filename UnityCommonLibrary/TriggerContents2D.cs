using System.Collections.Generic;
using UnityCommonLibrary.Messaging;
using UnityEngine;

namespace UnityCommonLibrary
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggerContents2D : MonoBehaviour
    {
        public readonly Message<bool, Collider2D> ContentsChanged = new Message<bool, Collider2D>();

        private readonly HashSet<Collider2D> _contents = new HashSet<Collider2D>();

        public HashSet<Collider2D> Contents
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

        public Collider2D Trigger { get; private set; }

        private void Awake()
        {
            Trigger = GetComponent<Collider2D>();
            if (!Trigger.isTrigger)
            {
                UCLCore.Logger.LogError("", "COLLIDER2D MUST BE TRIGGER!");
            }
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            _contents.Add(c);
            ContentsChanged.Publish(true, c);
        }

        private void OnTriggerExit2D(Collider2D c)
        {
            _contents.Remove(c);
            ContentsChanged.Publish(false, c);
        }
    }
}
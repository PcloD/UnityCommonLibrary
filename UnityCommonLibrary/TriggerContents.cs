﻿using System.Collections.Generic;
using UnityCommonLibrary.Messaging;
using UnityEngine;

namespace UnityCommonLibrary
{
    [RequireComponent(typeof(Collider))]
    public class TriggerContents : MonoBehaviour
    {
        public readonly Signal<bool, Collider> ContentsChanged = new Signal<bool, Collider>();

        private readonly HashSet<Collider> _contents = new HashSet<Collider>();

        public Collider Trigger { get; private set; }

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
            Trigger = GetComponent<Collider>();
            if (!Trigger.isTrigger)
            {
                UCLCore.Logger.LogError("", "COLLIDER MUST BE TRIGGER!");
            }
        }

        private void OnTriggerEnter(Collider c)
        {
            _contents.Add(c);
            ContentsChanged.Publish(true, c);
        }

        private void OnTriggerExit(Collider c)
        {
            _contents.Remove(c);
            ContentsChanged.Publish(false, c);
        }
    }
}
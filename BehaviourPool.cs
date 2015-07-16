using System;
using UnityEngine;

namespace UnityCommonLibrary {
    public class BehaviourPool<B> : AbstractPool<B> where B : Behaviour {
        public bool enableOnGet = true;
        bool disallowMultiple;
        GameObject root;

        public BehaviourPool(int initialCount) : base(initialCount) {
            disallowMultiple = Attribute.IsDefined(typeof(B), typeof(DisallowMultipleComponent));
        }

        protected override B CreateItem() {
            if(root == null) {
                root = new GameObject(GetType().Name);
            }

            B newInstance;
            if(disallowMultiple) {
                var parent = new GameObject(typeof(B).Name);
                parent.transform.parent = root.transform;
                newInstance = parent.AddComponent<B>();
            }
            else {
                newInstance = root.AddComponent<B>();
            }
            newInstance.enabled = false;
            return newInstance;
        }

        protected override void PostGetItem(ref B item) {
            item.enabled = enableOnGet;
        }

        protected override bool PreReturnItem(ref B item) {
            item.enabled = false;
            return true;
        }
    }
}
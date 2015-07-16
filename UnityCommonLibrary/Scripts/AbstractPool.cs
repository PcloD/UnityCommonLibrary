using UnityEngine;
using System.Collections.Generic;

namespace UnityCommonLibrary {
    public abstract class AbstractPool<I> : UCObject where I : Object {
        protected Queue<I> items;
        protected HashSet<int> spawned = new HashSet<int>();
        protected int initialCapacity;

        public AbstractPool(int initialCount) {
            items = new Queue<I>(initialCount);
            FillPool(initialCount);
        }

        void FillPool(int count) {
            for(var i = 0; i < count; i++) {
                var item = CreateItem();
                spawned.Add(item.GetInstanceID());
                items.Enqueue(item);
            }
        }

        protected abstract I CreateItem();

        public virtual I GetItem() {
            PreGetItem();
            if(items.Count == 0) {
                FillPool(1);
            }
            var item = items.Dequeue();
            PostGetItem(ref item);
            return item;
        }

        /// <summary>
        /// Releases an item from the scope of this pool.
        /// Item must not be null.
        /// </summary>
        public void Release(I item) {
            Release(item.GetInstanceID());
        }

        /// <summary>
        /// Releases an item via Instance ID from the scope of this pool.
        /// </summary>
        public void Release(int id) {
            spawned.Remove(id);
        }

        public void ReturnItem(I item) {
            if(spawned.Contains(item.GetInstanceID())) {
                if(PreReturnItem(ref item)) {
                    items.Enqueue(item);
                }
                PostReturnItem();
            }
        }

        protected virtual void PreGetItem() { }
        protected virtual void PostGetItem(ref I item) { }

        ///<summary>Returns true if return action is valid, false otherwise</summary>
        protected virtual bool PreReturnItem(ref I item) { return true; }
        protected virtual void PostReturnItem() { }
    }
}
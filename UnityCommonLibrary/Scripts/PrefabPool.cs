using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary {

    public abstract class AbstractPool<T> {
        readonly Stack pool = new Stack();

        public int countActive { get; protected set; }
        public int countInactive { get { return pool.Count; } }
        public int count { get { return countActive + countInactive; } }

        protected void InitFillPool(int initCount) {
            initCount = (initCount < 0) ? 0 : initCount;
            if(initCount > 0) {
                var objs = new T[initCount];
                for(int i = 0; i < initCount; i++) {
                    objs[i] = Get();
                }
                for(int i = 0; i < initCount; i++) {
                    Return(objs[i]);
                }
            }
        }

        public T[] Get(int count) {
            if(count <= 0) {
                throw new System.ArgumentOutOfRangeException("count");
            }
            var array = new T[count];
            for(int i = 0; i < count; i++) {
                array[i] = Get();
            }
            return array;
        }

        /// <summary>
        /// Same as <see cref="Get(int)"/> without allocating a new array
        /// </summary>
        public void Get(ref T[] array) {
            if(array == null) {
                throw new System.ArgumentNullException("array");
            }
            if(array.Length == 0) {
                throw new System.IndexOutOfRangeException("array.Length must be > 0");
            }
            for(int i = 0; i < array.Length; i++) {
                array[i] = Get();
            }
        }

        public T Get() {
            if(pool.Count == 0) {
                var newObj = CreateNew();
                ProcessNew(newObj);
                return newObj;
            }
            else {
                T obj = (T)pool.Pop();
                OnGetFromPool(obj);
                return obj;
            }
        }

        protected virtual T CreateNew() {
            return System.Activator.CreateInstance<T>();
        }
        protected virtual void ProcessNew(T obj) { }

        protected abstract void OnGetFromPool(T obj);
        protected abstract void OnReturnToPool(T obj);

        public void Return(T obj) {
            if(pool.Count > 0 && ReferenceEquals(pool.Peek(), obj)) {
                Debug.LogError("Trying to destroy object that is already released to pool.");
            }
            else {
                OnReturnToPool(obj);
                pool.Push(obj);
                countActive--;
            }
        }
    }

    public class PrefabPool : AbstractPool<GameObject> {
        public static GameObject poolParent { get; protected set; }
        protected GameObject prefab;

        public PrefabPool(GameObject prefab) : this(prefab, 0) { }
        public PrefabPool(GameObject prefab, int initCount) {
            this.prefab = prefab;
            InitFillPool(initCount);
        }

        protected override void ProcessNew(GameObject obj) {
            if(poolParent == null) {
                poolParent = new GameObject("PrefabPool", typeof(LogicalObject));
            }
            obj.transform.parent = poolParent.transform;
        }

        protected override GameObject CreateNew() {
            return Object.Instantiate(prefab);
        }

        protected sealed override void OnGetFromPool(GameObject obj) {
            obj.SetActive(true);
            obj.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
        }

        protected sealed override void OnReturnToPool(GameObject obj) {
            obj.SetActive(false);
            obj.SendMessage("OnDestroy", SendMessageOptions.DontRequireReceiver);
        }
    }

}
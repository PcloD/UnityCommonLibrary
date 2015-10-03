using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary {

    public abstract class AbstractPool<T> {
        readonly Stack pool = new Stack();

        public int countActive { get; protected set; }
        public int countInactive { get { return pool.Count; } }
        public int count { get { return countActive + countInactive; } }

        public AbstractPool(int initCount) {
            InitFillPool(initCount);
        }

        private void InitFillPool(int initCount) {
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

        public T Get() {
            if(pool.Count == 0) {
                return CreateNew();
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
        public readonly static GameObject poolParent = new GameObject("PrefabPool", typeof(LogicalObject));
        protected readonly GameObject prefab;

        public PrefabPool(GameObject prefab) : this(prefab, 0) { }
        public PrefabPool(GameObject prefab, int initCount) : base(initCount) {
            this.prefab = prefab;
        }

        protected override GameObject CreateNew() {
            var obj = Object.Instantiate(prefab);
            obj.transform.parent = poolParent.transform;
            return obj;
        }

        protected sealed override void OnGetFromPool(GameObject obj) {
            obj.SendMessage("OnEnable", SendMessageOptions.DontRequireReceiver);
            obj.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
            obj.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
        }

        protected sealed override void OnReturnToPool(GameObject obj) {
            obj.SendMessage("OnDisable", SendMessageOptions.DontRequireReceiver);
            obj.SendMessage("OnDestroy", SendMessageOptions.DontRequireReceiver);
        }
    }

}
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityCommonLibrary
{
    public abstract class AbstractPool<T>
    {
        private readonly Stack _pool = new Stack();

        public int Count
        {
            get { return CountActive + CountInactive; }
        }

        public int CountActive { get; protected set; }

        public int CountInactive
        {
            get { return _pool.Count; }
        }

        public T[] Get(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            var array = new T[count];
            for (var i = 0; i < count; i++)
            {
                array[i] = Get();
            }
            return array;
        }

        /// <summary>
        ///     Same as <see cref="Get(int)" /> without allocating a new array
        /// </summary>
        public void Get(ref T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Length == 0)
            {
                throw new IndexOutOfRangeException("array.Length must be > 0");
            }
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = Get();
            }
        }

        public T Get()
        {
            if (_pool.Count == 0)
            {
                var newObj = CreateNew();
                ProcessNew(newObj);
                return newObj;
            }
            var obj = (T) _pool.Pop();
            OnGetFromPool(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (_pool.Count > 0 && ReferenceEquals(_pool.Peek(), obj))
            {
                Debug.LogError(
                    "Trying to destroy object that is already released to pool.");
            }
            else
            {
                OnReturnToPool(obj);
                _pool.Push(obj);
                CountActive--;
            }
        }

        protected virtual T CreateNew()
        {
            return Activator.CreateInstance<T>();
        }

        protected void InitFillPool(int initCount)
        {
            initCount = initCount < 0 ? 0 : initCount;
            if (initCount > 0)
            {
                var objs = new T[initCount];
                for (var i = 0; i < initCount; i++)
                {
                    objs[i] = Get();
                }
                for (var i = 0; i < initCount; i++)
                {
                    Return(objs[i]);
                }
            }
        }

        protected abstract void OnGetFromPool(T obj);
        protected abstract void OnReturnToPool(T obj);
        protected virtual void ProcessNew(T obj) { }
    }

    public class PrefabPool : AbstractPool<GameObject>
    {
        protected GameObject Prefab;
        public static GameObject PoolParent { get; protected set; }

        public PrefabPool(GameObject prefab) : this(prefab, 0) { }

        public PrefabPool(GameObject prefab, int initCount)
        {
            Prefab = prefab;
            InitFillPool(initCount);
        }

        protected override GameObject CreateNew()
        {
            return Object.Instantiate(Prefab);
        }

        protected sealed override void OnGetFromPool(GameObject obj)
        {
            obj.SetActive(true);
            obj.SendMessage("Start", SendMessageOptions.DontRequireReceiver);
        }

        protected sealed override void OnReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.SendMessage("OnDestroy", SendMessageOptions.DontRequireReceiver);
        }

        protected override void ProcessNew(GameObject obj)
        {
            if (PoolParent == null)
            {
                PoolParent = new GameObject("PrefabPool", typeof(TransformLocker));
            }
            obj.transform.parent = PoolParent.transform;
        }
    }
}
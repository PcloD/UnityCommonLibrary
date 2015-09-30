using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary {
    public class PrefabPool {
        readonly static GameObject poolParent = new GameObject("PrefabPool", typeof(LogicalObject));

        readonly Stack<GameObject> m_Stack = new Stack<GameObject>();
        readonly GameObject prefab;

        public int countActive { get; private set; }
        public int countInactive { get { return m_Stack.Count; } }
        public int count { get { return countActive + countInactive; } }

        public PrefabPool(GameObject prefab) : this(prefab, 0) { }

        public PrefabPool(GameObject prefab, int initCount) {
            this.prefab = prefab;
            initCount = (initCount < 0) ? 0 : initCount;
            if(initCount > 0) {
                var objs = new GameObject[initCount];
                for(int i = 0; i < initCount; i++) {
                    objs[i] = Get();
                }
                for(int i = 0; i < initCount; i++) {
                    Return(objs[i]);
                }
            }
        }

        public GameObject Get() {
            GameObject obj;
            if(m_Stack.Count == 0) {
                obj = Object.Instantiate(prefab);
                obj.transform.parent = poolParent.transform;
                countActive++;
            }
            else {
                obj = m_Stack.Pop();
            }
            Observables.PrefabRetrievedFromPool.Notify(this, obj);
            return obj;
        }

        public void Return(GameObject obj) {
            if(m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), obj)) {
                Debug.LogError("Trying to destroy object that is already released to pool.");
            }
            else {
                Observables.PrefabReturnedToPool.Notify(this, obj);
                obj.SetActive(false);
                m_Stack.Push(obj);
                countActive--;
            }
        }
    }
}
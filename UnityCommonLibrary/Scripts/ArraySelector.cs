using System.Collections.Generic;
using System.Linq;
using UnityCommonLibrary.Utilities;
using UnityEngine;

namespace UnityCommonLibrary {
    public class ArraySelector<T>  {
        T[] array;
        int lastSelected;
        Queue<int> indiciesLeft = new Queue<int>();

        public ArraySelector(T[] array) {
            this.array = array;
        }

        void ResetIndexList() {
            if(array.Length == 1) {
                indiciesLeft = new Queue<int>();
                indiciesLeft.Enqueue(0);
            }
            else {
                var list = new List<int>(Enumerable.Range(0, array.Length - 1));
                list.Shuffle();
                indiciesLeft = new Queue<int>(list);
            }
        }

        private bool CheckArray() {
            if(array == null) {
                return false;
            }
            return true;
        }

        public T GetRandom() {
            if(CheckArray()) {
                return default(T);
            }
            return array[Random.Range(0, array.Length)];
        }

        public T GetRandomNew() {
            if(!CheckArray()) {
                return default(T);
            }

            var index = 0;
            do index = Random.Range(0, array.Length - 1);
            while(index == lastSelected && array.Length > 1);
            return array[index];
        }

        public T GetRandomUnique() {
            if(!CheckArray()) {
                return default(T);
            }
            if(indiciesLeft.Count == 0) {
                ResetIndexList();
            }
            var index = indiciesLeft.Dequeue();
            return array[index];
        }
    }
}
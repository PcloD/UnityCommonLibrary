using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UnityCommonLibrary {
    public class ArraySelector<T> : UCObject {
        T[] array;
        int lastSelected;
        List<int> indiciesLeft = new List<int>();

        public ArraySelector(ref T[] array) {
            this.array = array;
        }

        public ArraySelector(T[] array) {
            this.array = array;
        }

        void ResetIndexList() {
            if(array.Length == 1) {
                indiciesLeft = new List<int>();
                indiciesLeft.Add(0);
            }
            else {
                indiciesLeft = new List<int>(Enumerable.Range(0, array.Length - 1));
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
            return array[Random.Range(0, array.Length - 1)];
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
            var index = indiciesLeft[Random.Range(0, indiciesLeft.Count - 1)];
            return array[index];
        }
    }
}
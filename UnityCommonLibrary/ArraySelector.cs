using System.Collections.Generic;
using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
    public class ArraySelector<T>
    {
        private readonly List<T> _shuffled = new List<T>();
        private T[] _array;
        private int _lastSelected;

        public ArraySelector(T[] array)
        {
            UpdateArray(array);
        }

        public T GetRandom()
        {
            return _shuffled[Random.Range(0, _shuffled.Count)];
        }

        public T GetRandomNew()
        {
            int index;
            do
            {
                index = Random.Range(0, _array.Length - 1);
            } while (index == _lastSelected && _array.Length > 1);
            _lastSelected = index;
            return _array[index];
        }

        public T GetRandomUnique()
        {
            if (_shuffled.Count == 0)
            {
                RefillShuffledList();
            }
            var value = _shuffled[_shuffled.Count - 1];
            _shuffled.RemoveAt(_shuffled.Count - 1);
            return value;
        }

        public void UpdateArray(T[] array)
        {
            if (array != null && array.Length > 0)
            {
                _array = array;
                RefillShuffledList();
            }
        }

        private void RefillShuffledList()
        {
            _shuffled.Clear();
            _shuffled.AddRange(_array);
            _shuffled.Shuffle();
        }
    }
}
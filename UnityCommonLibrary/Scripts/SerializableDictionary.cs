using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
    /// <summary>
    /// Not really a dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class OrderedSerializedDictionary<TKey, TValue>
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();
        [SerializeField]
        private List<TValue> values = new List<TValue>();

        public int length
        {
            get
            {
                return keys.Count;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return keys.Contains(key);
        }

        public bool ContainsValue(TValue value)
        {
            return values.Contains(value);
        }

        public KeyValuePair<TKey, TValue> this[int index]
        {
            get { return new KeyValuePair<TKey, TValue>(keys[index], values[index]); }
        }

        public TValue this[TKey key]
        {
            get { return values[keys.IndexOf(key)]; }
        }

        public void Add(TKey key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public void Set(TKey key, TValue value)
        {
            Set(keys.IndexOf(key), value);
        }

        public void Set(int index, TValue value)
        {
            values[index] = value;
        }

        public void Remove(TKey key)
        {
            var index = keys.IndexOf(key);
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            keys.RemoveAt(index);
            values.RemoveAt(index);
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using URandom = UnityEngine.Random;

namespace UnityCommonLibrary.Utilities
{
    public static class CollectionUtility {

        public static void Shuffle<T>(this IList<T> list) {
            var n = list.Count;
            while(n > 1) {
                n--;
                var k = URandom.Range(0, n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T Random<T>(this IEnumerable<T> enumerable) {
            if(enumerable == null) {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var list = enumerable as IList<T>;
            if(list == null) {
                list = enumerable.ToList();
            }
            return list.Count == 0 ? default(T) : list[URandom.Range(0, list.Count)];
        }

        public static T[] Random<T>(this IEnumerable<T> enumerable, int count) {
            if(enumerable == null) {
                throw new ArgumentNullException(nameof(enumerable));
            }
            var array = new T[count];
            for(int i = 0; i < count; i++) {
                array[i] = enumerable.Random();
            }
            return array;
        }

    }
}
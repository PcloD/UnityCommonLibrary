using System.Collections.Generic;
using System.Linq;
using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
	public class ArraySelector<T>
	{
		private T[] array;
		private int lastSelected;
		private List<T> shuffled = new List<T>();

		public ArraySelector(T[] array)
		{
			UpdateArray(array);
		}

		public T GetRandom()
		{
			return shuffled[Random.Range(0, shuffled.Count)];
		}
		public T GetRandomNew()
		{
			var index = 0;
			do index = Random.Range(0, array.Length - 1);
			while (index == lastSelected && array.Length > 1);
			return array[index];
		}
		public T GetRandomUnique()
		{
			if (shuffled.Count == 0)
			{
				RefillShuffledList();
			}
			var value = shuffled[shuffled.Count - 1];
			shuffled.RemoveAt(shuffled.Count - 1);
			return value;
		}
		public void UpdateArray(T[] array)
		{
			if (array != null && array.Length > 0)
			{
				this.array = array;
				RefillShuffledList();
			}
		}

		private void RefillShuffledList()
		{
			shuffled.Clear();
			shuffled.AddRange(array);
			shuffled.Shuffle();
		}
	}
}
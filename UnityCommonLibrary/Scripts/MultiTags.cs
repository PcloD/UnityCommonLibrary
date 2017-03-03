using System;
using System.Collections.Generic;
using UnityCommonLibrary;
using UnityEngine;

namespace UnityCommonLibrary
{
	public static class MultiTags<T> where T : struct, IFormattable, IConvertible, IComparable
	{
		private static readonly Dictionary<GameObject, T> lookup = new Dictionary<GameObject, T>();
		private static readonly T noneValue = FromInt(0);

		public static bool HasAnyTags(Component cmp, T mask)
		{
			return HasAnyTags(cmp.gameObject, mask);
		}
		public static bool HasAnyTags(GameObject obj, T mask)
		{
			var found = noneValue;
			if(!lookup.TryGetValue(obj, out found))
			{
				lookup.Add(obj, noneValue);
				return false;

			}
			var foundVal = ToInt(found);
			var maskVal = ToInt(mask);
			for(int i = 0; i < EnumData<T>.values.Length; i++)
			{
				var v = ToInt(EnumData<T>.values[i]);
				if((maskVal & v) == v)
				{
					if((foundVal & v) == v)
					{
						return true;
					}
				}
			}
			return false;
		}
		public static bool HasTags(Component cmp, T tag)
		{
			return HasTags(cmp.gameObject, tag);
		}
		public static bool HasTags(GameObject obj, T mask)
		{
			var found = noneValue;
			if(!lookup.TryGetValue(obj, out found))
			{
				lookup.Add(obj, noneValue);
				return false;

			}
			var maskVal = ToInt(mask);
			var foundVal = ToInt(found);
			for(int i = 0; i < EnumData<T>.values.Length; i++)
			{
				var v = ToInt(EnumData<T>.values[i]);
				if((maskVal & v) == v)
				{
					if((foundVal & v) != v)
					{
						return false;
					}
				}
			}
			return true;
		}
		public static T GetTags(Component cmp)
		{
			return GetTags(cmp.gameObject);
		}
		public static T GetTags(GameObject obj)
		{
			var found = noneValue;
			if(!lookup.TryGetValue(obj, out found))
			{
				lookup.Add(obj, noneValue);
			}
			return found;
		}
		public static void SetTags(Component cmp, T tags)
		{
			SetTags(cmp.gameObject, tags);
		}
		public static void SetTags(GameObject obj, T tags)
		{
			if(lookup.ContainsKey(obj))
			{
				lookup[obj] = tags;
			}
			else
			{
				lookup.Add(obj, tags);
			}
		}
		public static void AddTags(Component cmp, T mask)
		{
			AddTags(cmp.gameObject, mask);
		}
		public static void AddTags(GameObject obj, T mask)
		{
			if(lookup.ContainsKey(obj))
			{
				var maskVal = ToInt(lookup[obj]);
				maskVal |= ToInt(mask);
				lookup[obj] = FromInt(maskVal);
			}
			else
			{
				lookup.Add(obj, mask);
			}
		}
		public static void RemoveTags(Component cmp, T mask)
		{
			RemoveTags(cmp.gameObject, mask);
		}
		public static void RemoveTags(GameObject obj, T mask)
		{
			if(lookup.ContainsKey(obj))
			{
				var maskVal = ToInt(lookup[obj]);
				maskVal &= ~ToInt(mask);
				lookup[obj] = FromInt(maskVal);
			}
			else
			{
				lookup.Add(obj, noneValue);
			}
		}

		public static bool TagsMatch(Component cmp, Component other)
		{
			return TagsMatch(cmp.gameObject, other.gameObject);
		}
		public static bool TagsMatch(Component cmp, GameObject other)
		{
			return TagsMatch(cmp.gameObject, other);
		}
		public static bool TagsMatch(GameObject obj, Component other)
		{
			return TagsMatch(obj, other.gameObject);
		}
		public static bool TagsMatch(GameObject obj, GameObject other)
		{
			return GetTags(obj).Equals(GetTags(other));
		}

		public static bool SharesAny(Component cmp, Component other)
		{
			return SharesAny(cmp.gameObject, other.gameObject);
		}
		public static bool SharesAny(Component cmp, GameObject other)
		{
			return SharesAny(cmp.gameObject, other);
		}
		public static bool SharesAny(GameObject obj, Component other)
		{
			return SharesAny(obj, other.gameObject);
		}
		public static bool SharesAny(GameObject obj, GameObject other)
		{
			for(int i = 0; i < EnumData<T>.values.Length; i++)
			{
				var v = EnumData<T>.values[i];
				if(SharesTag(obj, other, v))
				{
					return true;
				}
			}
			return false;
		}

		public static bool SharesTag(Component cmp, Component other, T tag)
		{
			return SharesTag(cmp.gameObject, other.gameObject, tag);
		}
		public static bool SharesTag(Component cmp, GameObject other, T tag)
		{
			return SharesTag(cmp.gameObject, other, tag);
		}
		public static bool SharesTag(GameObject obj, Component other, T tag)
		{
			return SharesTag(obj, other.gameObject, tag);
		}
		public static bool SharesTag(GameObject obj, GameObject other, T tag)
		{
			return HasTags(obj, tag) && HasTags(other, tag);
		}
		public static GameObject GetFirstWithTag(T tag)
		{
			foreach(var kvp in lookup)
			{
				if(HasTags(kvp.Key, tag))
				{
					return kvp.Key;
				}
			}
			return null;
		}
		public static List<GameObject> GetWithTag(T tag)
		{
			var list = new List<GameObject>();
			foreach(var kvp in lookup)
			{
				if(HasTags(kvp.Key, tag))
				{
					list.Add(kvp.Key);
				}
			}
			return list;
		}

		private static int ToInt(T t)
		{
			return Convert.ToInt32(t);
		}
		private static T FromInt(int i)
		{
			return (T)Enum.ToObject(EnumData<T>.type, i);
		}
	}
}

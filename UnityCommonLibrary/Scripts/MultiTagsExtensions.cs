using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
	public static class MultiTagsExtensions
	{
		public static bool HasAnyTags<T>(this Component cmp, T mask) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.HasAnyTags(cmp, mask);
		}
		public static bool HasAnyTags<T>(this GameObject obj, T mask) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.HasAnyTags(obj, mask);
		}
		public static bool HasTags<T>(this Component cmp, T tag) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.HasTags(cmp, tag);
		}
		public static bool HasTags<T>(this GameObject obj, T mask) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.HasTags(obj, mask);
		}
		public static T GetTags<T>(this Component cmp) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.GetTags(cmp);
		}
		public static T GetTags<T>(this GameObject obj) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.GetTags(obj);
		}
		public static void SetTags<T>(this Component cmp, T tags) where T : struct, IFormattable, IConvertible, IComparable
		{
			MultiTags<T>.SetTags(cmp, tags);
		}
		public static void SetTags<T>(this GameObject obj, T tags) where T : struct, IFormattable, IConvertible, IComparable
		{
			MultiTags<T>.SetTags(obj, tags);
		}
		public static void AddTags<T>(this Component cmp, T mask) where T : struct, IFormattable, IConvertible, IComparable
		{
			MultiTags<T>.AddTags(cmp, mask);
		}
		public static void AddTags<T>(this GameObject obj, T mask) where T : struct, IFormattable, IConvertible, IComparable
		{
			MultiTags<T>.AddTags(obj, mask);
		}
		public static void RemoveTags<T>(this Component cmp, T mask) where T : struct, IFormattable, IConvertible, IComparable
		{
			MultiTags<T>.RemoveTags(cmp, mask);
		}
		public static void RemoveTags<T>(this GameObject obj, T mask) where T : struct, IFormattable, IConvertible, IComparable
		{
			MultiTags<T>.RemoveTags(obj, mask);
		}

		public static bool TagsMatch<T>(this Component cmp, Component other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.TagsMatch(cmp, other);
		}
		public static bool TagsMatch<T>(this Component cmp, GameObject other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.TagsMatch(cmp, other);
		}
		public static bool TagsMatch<T>(this GameObject obj, Component other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.TagsMatch(obj, other);
		}
		public static bool TagsMatch<T>(this GameObject obj, GameObject other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.TagsMatch(obj, other);
		}

		public static bool SharesAny<T>(this Component cmp, Component other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesAny(cmp, other);
		}
		public static bool SharesAny<T>(this Component cmp, GameObject other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesAny(cmp, other);
		}
		public static bool SharesAny<T>(this GameObject obj, Component other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesAny(obj, other);
		}
		public static bool SharesAny<T>(this GameObject obj, GameObject other) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesAny(obj, other);
		}

		public static bool SharesTag<T>(this Component cmp, Component other, T tag) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesTag(cmp, other, tag);
		}
		public static bool SharesTag<T>(this Component cmp, GameObject other, T tag) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesTag(cmp, other, tag);
		}
		public static bool SharesTag<T>(this GameObject obj, Component other, T tag) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesTag(obj, other, tag);
		}
		public static bool SharesTag<T>(this GameObject obj, GameObject other, T tag) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.SharesTag(obj, other, tag);
		}
		public static List<GameObject> GetWithTag<T>(T tag) where T : struct, IFormattable, IConvertible, IComparable
		{
			return MultiTags<T>.GetWithTag(tag);
		}
	}
}

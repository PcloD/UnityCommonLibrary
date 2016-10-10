using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityCommonLibrary
{
	public static class EnumData
	{
		private static Dictionary<Type, Array> lookup = new Dictionary<Type, Array>();

		public static T[] GetValues<T>() where T : struct, IFormattable, IConvertible, IComparable
		{
			var t = typeof(T);
			Array raw;
			if(!lookup.TryGetValue(t, out raw))
			{
				raw = Enum.GetValues(t);
				lookup.Add(t, raw);
			}
			return (T[])raw;
		}
	}
}

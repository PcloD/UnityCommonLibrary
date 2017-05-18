using System;

namespace UnityCommonLibrary
{
	public static class EnumData<T> where T : struct, IComparable, IFormattable, IConvertible
	{
		public static readonly Type type;
		public static readonly T[] values;
		public static readonly string[] names;

		public static int Count
		{
			get
			{
				return values.Length;
			}
		}

		static EnumData()
		{
			type = typeof(T);
			if(!type.IsEnum)
			{
				throw new Exception("Type T must be enum.");
			}
			values = (T[])Enum.GetValues(type);
			names = Enum.GetNames(type);
		}

		public static string GetName(T value)
		{
			return names[Array.IndexOf(values, value)];
		}
		public static T GetValue(string name)
		{
			return values[Array.IndexOf(names, name)];
		}
	}
}

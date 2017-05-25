using System;

namespace UnityCommonLibrary
{
    public static class EnumData<T>
        where T : struct, IComparable, IFormattable, IConvertible
    {
        public static readonly string[] Names;
        public static readonly Type Type;
        public static readonly T[] Values;

        public static int Count
        {
            get { return Values.Length; }
        }

        static EnumData()
        {
            Type = typeof(T);
            if (!Type.IsEnum)
            {
                throw new Exception("Type T must be enum.");
            }
            Values = (T[]) Enum.GetValues(Type);
            Names = Enum.GetNames(Type);
        }

        public static string GetName(T value)
        {
            return Names[Array.IndexOf(Values, value)];
        }

        public static T GetValue(string name)
        {
            return Values[Array.IndexOf(Names, name)];
        }
    }
}
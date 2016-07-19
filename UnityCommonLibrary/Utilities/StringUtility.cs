namespace UnityCommonLibrary.Utilities
{
    public static class StringUtility
    {
        public static bool IsNullOrWhitespace(string str)
        {
            if (str == null)
            {
                return true;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsWhiteSpace(str[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool ContainsIgnoreCase(this string str, string other)
        {
            return str.IndexOf(other, System.StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}

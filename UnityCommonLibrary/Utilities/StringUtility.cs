namespace UnityCommonLibrary.Utilities
{
    public static class StringUtility
    {

        public static bool IsNullOrWhitespace(string str)
        {
            return str == null || str.Trim().Length == 0;
        }
        public static bool ContainsCaseInsensitive(this string str, string other)
        {
            return str.IndexOf(other, System.StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}

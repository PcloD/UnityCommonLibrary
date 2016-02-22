namespace UnityCommonLibrary.Utilities {
    public static class StringUtility {

        public static bool IsNullOrWhitespace(string str) {
            return str == null || str.Trim().Length == 0;
        }
    }
}

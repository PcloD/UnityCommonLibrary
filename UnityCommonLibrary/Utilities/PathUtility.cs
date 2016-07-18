using System.IO;

namespace UnityCommonLibrary.Utilities
{
    public static class PathUtility
    {

        public static string Combine(params string[] paths)
        {
            var path = string.Empty;
            foreach (var p in paths)
            {
                path = Path.Combine(path, p);
            }
            return path;
        }

    }
}
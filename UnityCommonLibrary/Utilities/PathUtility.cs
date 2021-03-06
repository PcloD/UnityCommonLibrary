﻿using System.IO;

namespace UnityCommonLibrary.Utility
{
    public static class PathUtility
    {
        public static string Combine(params string[] paths)
        {
            var path = "";
            foreach (var p in paths)
            {
                path = Path.Combine(path, p);
            }
            return path;
        }
    }
}
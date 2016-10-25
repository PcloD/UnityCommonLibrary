using System;
using UnityEngine;

namespace UnityCommonLibrary.Utility
{
    public static class TextAssetUtility
    {
        private static readonly string[] newline = new string[] { Environment.NewLine };

        public static string[] GetLines(this TextAsset asset)
        {
            return asset.text.Split(newline, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
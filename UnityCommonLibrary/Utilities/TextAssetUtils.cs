using System;
using UnityEngine;

namespace UnityCommonLibrary.Utility
{
    public static class TextAssetUtility
    {
        private static readonly string[] _newline = {Environment.NewLine};

        public static string[] GetLines(this TextAsset asset)
        {
            return asset.text.Split(_newline, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
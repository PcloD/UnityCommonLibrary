using System;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class TextAssetUtils {
        static readonly string[] newlineArray = new string[] { "\r\n", "\n" };

        public static string[] GetLines(this TextAsset asset) {
            return asset.text.Split(newlineArray, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
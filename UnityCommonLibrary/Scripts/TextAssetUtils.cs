using System;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class TextAssetUtils {

        public static string[] GetLines(this TextAsset asset) {
            return asset.text.Split(Environment.NewLine[0]);
        }

    }
}

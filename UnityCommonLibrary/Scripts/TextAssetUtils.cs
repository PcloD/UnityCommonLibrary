using System;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class TextAssetUtils {


        public static string[] GetLines(this TextAsset text) {
            return text.text.Split(Environment.NewLine[0]);
        }



    }
}

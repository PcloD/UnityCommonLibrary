using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary {
    [Serializable]
    public class GameTextDictionary : OrderedSerializedDictionary<Language, string> { }

    [Serializable]
    public class GameTextLine {
        public string uniqueName = Guid.NewGuid().ToString();
        public GameTextDictionary dict = new GameTextDictionary();
    }

    [Serializable]
    [ScriptableAssetWizard]
    public class GameText : ScriptableObject {
        public string category;
        public List<GameTextLine> lines = new List<GameTextLine>();
        public static Language defaultLang = Language.English;

        public string GetLine(int id) {
            return GetLine(id, defaultLang);
        }

        public string GetLine(string uniqueName) {
            return GetLine(uniqueName, defaultLang);
        }

        public string GetLine(int id, Language lang) {
            return lines[id].dict[lang];
        }

        public string GetLine(string uniqueName, Language lang) {
            return lines.Find(l => l.uniqueName == uniqueName).dict[lang];
        }

        public string GetRandom() {
            return GetLine(UnityEngine.Random.Range(0, lines.Count - 1)); ;
        }

        public string GetRandom(Language lang) {
            return GetLine(UnityEngine.Random.Range(0, lines.Count - 1), lang);
        }
    }

    //That's it.
    public enum Language {
        English,
        Swedish
    }
}
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Linq;

namespace UnityCommonEditorLibrary {
    public class StaticsGenerator : Editor {
        const string NEW_LINE = ",\r\n\t\t";

        [MenuItem("Assets/Generate Statics")]
        static void Execute() {
            var layers = InternalEditorUtility.layers.Select(l => MakeProperIdentifier(l)).ToArray();
            var tags = InternalEditorUtility.tags.Select(t => MakeProperIdentifier(t)).ToArray();
            var scenes = EditorBuildSettings.scenes.Select(s => MakeProperIdentifier(Path.GetFileNameWithoutExtension(s.path))).ToArray();

            var lString = string.Join(NEW_LINE, layers);
            var tStr = string.Join(NEW_LINE, tags);
            var sStr = string.Join(NEW_LINE, scenes);

            var final = Properties.Resources.StaticGenerator;
            final = string.Format(final, lString, tStr, sStr);
            File.WriteAllText(string.Format("{0}/Statics.cs", Application.dataPath), final);
            AssetDatabase.Refresh();
        }

        private static string MakeProperIdentifier(string str) {
            str = str.Trim();
            if(!char.IsLetter(str[0])) {
                str = str.Insert(0, "_");
            }
            str = Regex.Replace(str, @"\s+", "_");
            str = Regex.Replace(str, @"[^A-Za-z0-9_]", "");
            return str;
        }

    }
}

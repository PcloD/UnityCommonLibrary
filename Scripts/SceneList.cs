using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityCommonLibrary {
#if UNITY_EDITOR
    public class SceneList : ScriptableWizard {
        static Dictionary<string, string> scenes = new Dictionary<string, string>();

        [MenuItem("Tools/Generate Level List")]
        static void GenerateList() {
            DisplayWizard<SceneList>("Scene List Generator");
        }

        void OnWizardCreate() {
            var path = EditorUtility.SaveFilePanelInProject("Save Level List", "LevelList", "bytes", "Save");
            if(path == null) {
                return;
            }
            var enabledScenes = EditorBuildSettings.scenes.Where(s => s.enabled).ToArray();
            using(var bw = new BinaryWriter(File.OpenWrite(path))) {
                bw.Write(enabledScenes.Length);
                foreach(var s in enabledScenes) {
                    var name = Path.GetFileNameWithoutExtension(s.path);
                    bw.Write(name);
                    bw.Write(scenes[name]);
                }
            }
            AssetDatabase.Refresh();
        }

        protected override bool DrawWizardGUI() {
            var enabledScenes = EditorBuildSettings.scenes.Where(s => s.enabled).ToArray();

            EditorGUILayout.BeginVertical();
            foreach(var s in enabledScenes) {
                var name = Path.GetFileNameWithoutExtension(s.path);
                if(!scenes.ContainsKey(name)) {
                    scenes.Add(name, name);
                }
                scenes[name] = EditorGUILayout.TextField(name, scenes[name]);
            }
            EditorGUILayout.EndVertical();
            return false;
        }

    }
#endif

    public class Scene : UCObject {
        public static List<Scene> all = new List<Scene>();
        public string rawName { get; private set; }
        public string displayName { get; private set; }

        public Scene(string path, string displayName) {
            this.rawName = path;
            this.displayName = displayName;
        }

        public static void LoadList(TextAsset text) {
            all.Clear();

            using(var bw = new BinaryReader(new MemoryStream(text.bytes))) {
                var count = bw.ReadInt32();
                for(var i = 0; i < count; i++) {
                    all.Add(new Scene(bw.ReadString(), bw.ReadString()));
                }
            }

        }

    }
}
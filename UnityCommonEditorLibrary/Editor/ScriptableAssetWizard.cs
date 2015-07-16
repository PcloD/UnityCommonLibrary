using System;
using System.Linq;
using System.Reflection;
using UnityCommonLibrary;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    public class ScriptableAssetWizard : ScriptableWizard {
        int selectedTypeIndex = 0;
        int projectAssemblyIndex;

        Assembly assembly;
        string startPath;

        Type[] types;
        string[] typeNames;

        [MenuItem("Tools/Create Scriptable Asset...")]
        public static void CreateWizard() {
            var wizard = DisplayWizard<ScriptableAssetWizard>("Scriptable Asset Wizard");
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            wizard.startPath = (path != null) ? path : "Assets";
            wizard.assembly = GetProjectAssembly();
            wizard.GenerateChoices();
        }

        [MenuItem("Assets/Create/Scriptable Asset...", priority = -999)]
        public static void CreateWizardAlt() {
            CreateWizard();
        }

        private static Assembly GetProjectAssembly() {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .First(a => a.FullName.StartsWith("Assembly-CSharp,"));
        }

        private static bool IsCorrectType(Type t) {
            return !t.IsAbstract &&
                    t.IsSubclassOf(typeof(ScriptableObject)) &&
                   !t.IsSubclassOf(typeof(UnityEditor.Editor)) &&
                   !t.IsSubclassOf(typeof(EditorWindow)) &&
                   t.GetCustomAttributes(false).Any(a => a is ScriptableAssetWizardAttribute);
        }

        private void GenerateChoices() {
            types = assembly.GetTypes().Where(t => IsCorrectType(t)).ToArray();
            typeNames = types.Select(t => t.Name).ToArray();
        }

        void OnWizardCreate() {
            var type = types[selectedTypeIndex];

            var path = EditorUtility.SaveFilePanel("Save location", startPath, "New " + type.Name, "asset");

            if(string.IsNullOrEmpty(path)) {
                return;
            }

            //Get project relative path and ensure path is within project
            var projectRelative = FileUtil.GetProjectRelativePath(path);
            if(string.IsNullOrEmpty(projectRelative)) {
                EditorUtility.DisplayDialog("Error", "Please select somewhere within your assets folder.", "OK");
                return;
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(projectRelative);

            var scriptableObject = CreateInstance(type);
            AssetDatabase.CreateAsset(scriptableObject, assetPathAndName);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = scriptableObject;
        }

        protected override bool DrawWizardGUI() {
            var changed = false;
            var newIndex = -1;

            if(changed) {
                GenerateChoices();
            }

            newIndex = EditorGUILayout.Popup("ScriptableObject", selectedTypeIndex, typeNames);
            changed |= newIndex != selectedTypeIndex;
            selectedTypeIndex = newIndex;

            return changed;
        }
    }
}
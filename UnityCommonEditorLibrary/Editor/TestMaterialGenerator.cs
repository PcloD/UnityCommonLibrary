using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    public class TestMaterialGenerator : ScriptableWizard {
        static TestMaterialGenerator wizard;
        static string saveFolder, projRelativeSaveFolder, filename = "TestMaterial";
        static bool isFoldout = true;

        public static Color32 primary = new Color32(255, 255, 255, 255);
        public static Color32 secondary = new Color32(190, 190, 190, 255);
        public static List<Vector2> scales = new List<Vector2>();

        Texture2D texture;

        [MenuItem("Assets/Create/Test Materials")]
        static void CreateWizard() {
            wizard = DisplayWizard<TestMaterialGenerator>("Create Test Material", "Create");
            if(scales.Count == 0) {
                scales.Add(Vector2.one);
                scales.Add(Vector2.one * 2f);
                scales.Add(Vector2.one * 5f);
            }
            if(saveFolder == null) {
                saveFolder = Application.dataPath;
            }
        }

        void OnWizardCreate() {
            CreateTexture();
            CreateMaterials();
        }

        void CreateMaterials() {
            var distinct = scales.Distinct();
            foreach(var s in distinct) {
                var newMat = new Material(Shader.Find("Standard"));
                newMat.mainTexture = texture;
                newMat.mainTextureScale = s;
                AssetDatabase.CreateAsset(newMat, string.Format("{0}/{1} {2}x{3}.mat", projRelativeSaveFolder, filename, s.x, s.y));
            }
        }

        protected override bool DrawWizardGUI() {
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            DrawFileInfo();
            DrawColorFields();
            DrawScales();
            UpdateValidity();

            return EditorGUI.EndChangeCheck();
        }

        void DrawScales() {
            isFoldout = EditorGUILayout.Foldout(isFoldout, "Scales");
            if(isFoldout) {
                EditorGUI.indentLevel = 1;
                var newSize = EditorGUILayout.IntField("Size", scales.Count);
                if(newSize != scales.Count) {
                    ResizeList(scales, newSize);
                }
                for(var i = 0; i < scales.Count; i++) {
                    scales[i] = EditorGUILayout.Vector2Field("Element " + i, scales[i]);
                }
                EditorGUI.indentLevel = 0;
            }
        }

        void ResizeList<T>(List<T> list, int count) {
            count = Mathf.Clamp(count, 0, int.MaxValue);
            while(list.Count > count) {
                list.RemoveAt(list.Count - 1);
            }
            while(list.Count < count) {
                if(list.Count > 0) {
                    list.Add(list[list.Count - 1]);
                }
                else {
                    list.Add(default(T));
                }
            }
        }

        void DrawColorFields() {
            primary = EditorGUILayout.ColorField("Primary", primary);
            secondary = EditorGUILayout.ColorField("Secondary", secondary);
        }

        void UpdateValidity() {
            if(projRelativeSaveFolder == null || projRelativeSaveFolder.Length == 0) {
                errorString = "Must be saved in project.";
                isValid = false;
            }
            else {
                errorString = "";
                isValid = true;
            }
        }

        private static void DrawFileInfo() {
            if(GUILayout.Button("Browse")) {
                var newSaveFolder = EditorUtility.SaveFolderPanel("", projRelativeSaveFolder, "");
                saveFolder = newSaveFolder == null || newSaveFolder.Length == 0 ? saveFolder : newSaveFolder;
                GUI.changed = true;
            }
            projRelativeSaveFolder = FileUtil.GetProjectRelativePath(saveFolder);

            GUI.enabled = false;
            EditorGUILayout.TextField("Save Folder", projRelativeSaveFolder);
            GUI.enabled = true;

            filename = EditorGUILayout.TextField("Filename", filename);
        }

        private void CreateTexture() {
            var texPath = string.Format("{0}/{1}.png", saveFolder, filename);

            // Clamp alpha
            primary.a = secondary.a = 255;

            // Create texture
            texture = new Texture2D(2, 2);
            texture.filterMode = FilterMode.Point;
            texture.SetPixels32(new Color32[] { primary, secondary, secondary, primary });
            texture.Apply();

            // Write to path
            File.WriteAllBytes(texPath, texture.EncodeToPNG());
            DestroyImmediate(texture);
            AssetDatabase.Refresh();

            texPath = string.Format("{0}/{1}.png", projRelativeSaveFolder, filename);
            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);

            // Change import settings
            var importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
            importer.filterMode = FilterMode.Point;
            importer.maxTextureSize = 32;
            importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.textureType = TextureImporterType.Image;
            importer.SaveAndReimport();
        }

    }
}
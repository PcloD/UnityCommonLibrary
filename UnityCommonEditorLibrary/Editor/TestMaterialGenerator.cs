using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    public class TestMaterialGenerator : ScriptableWizard
    {
        public static Color32 Primary = new Color32(255, 255, 255, 255);
        public static List<Vector2> Scales = new List<Vector2>();
        public static Color32 Secondary = new Color32(190, 190, 190, 255);
        private static bool _isFoldout = true;

        private static string _saveFolder,
            _projRelativeSaveFolder,
            _filename = "TestMaterial";

        private static TestMaterialGenerator _wizard;

        private Texture2D _texture;

        [MenuItem("Assets/Create/Test Materials")]
        private static void CreateWizard()
        {
            _wizard = DisplayWizard<TestMaterialGenerator>("Create Test Material",
                "Create");
            if (Scales.Count == 0)
            {
                Scales.Add(Vector2.one);
                Scales.Add(Vector2.one * 2f);
                Scales.Add(Vector2.one * 5f);
            }
            if (_saveFolder == null)
            {
                _saveFolder = Application.dataPath;
            }
        }

        private static void DrawFileInfo()
        {
            if (GUILayout.Button("Browse"))
            {
                var newSaveFolder =
                    EditorUtility.SaveFolderPanel("", _projRelativeSaveFolder, "");
                _saveFolder = string.IsNullOrEmpty(newSaveFolder)
                    ? _saveFolder
                    : newSaveFolder;
                GUI.changed = true;
            }
            _projRelativeSaveFolder = FileUtil.GetProjectRelativePath(_saveFolder);

            GUI.enabled = false;
            EditorGUILayout.TextField("Save Folder", _projRelativeSaveFolder);
            GUI.enabled = true;

            _filename = EditorGUILayout.TextField("Filename", _filename);
        }

        private static void DrawColorFields()
        {
            Primary = EditorGUILayout.ColorField("Primary", Primary);
            Secondary = EditorGUILayout.ColorField("Secondary", Secondary);
        }

        private static void DrawScales()
        {
            _isFoldout = EditorGUILayout.Foldout(_isFoldout, "Scales");
            if (_isFoldout)
            {
                EditorGUI.indentLevel = 1;
                var newSize = EditorGUILayout.IntField("Size", Scales.Count);
                if (newSize != Scales.Count)
                {
                    ResizeList(Scales, newSize);
                }
                for (var i = 0; i < Scales.Count; i++)
                {
                    Scales[i] = EditorGUILayout.Vector2Field("Element " + i, Scales[i]);
                }
                EditorGUI.indentLevel = 0;
            }
        }

        private static void ResizeList<T>(List<T> list, int count)
        {
            count = Mathf.Clamp(count, 0, int.MaxValue);
            while (list.Count > count)
            {
                list.RemoveAt(list.Count - 1);
            }
            while (list.Count < count)
            {
                list.Add(list.Count > 0 ? list[list.Count - 1] : default(T));
            }
        }

        protected override bool DrawWizardGUI()
        {
            EditorGUILayout.Space();

            EditorGUI.BeginChangeCheck();
            DrawFileInfo();
            DrawColorFields();
            DrawScales();
            UpdateValidity();

            return EditorGUI.EndChangeCheck();
        }

        private void CreateMaterials()
        {
            var distinct = Scales.Distinct();
            foreach (var s in distinct)
            {
                var newMat =
                    new Material(Shader.Find("Standard"))
                    {
                        mainTexture = _texture,
                        mainTextureScale = s
                    };
                AssetDatabase.CreateAsset(newMat,
                    string.Format("{0}/{1} {2}x{3}.mat", _projRelativeSaveFolder,
                        _filename,
                        s.x, s.y));
            }
        }

        private void CreateTexture()
        {
            var texPath = string.Format("{0}/{1}.png", _saveFolder, _filename);

            // Clamp alpha
            Primary.a = Secondary.a = 255;

            // Create texture
            _texture = new Texture2D(2, 2) {filterMode = FilterMode.Point};
            _texture.SetPixels32(new[] {Primary, Secondary, Secondary, Primary});
            _texture.Apply();

            // Write to path
            File.WriteAllBytes(texPath, _texture.EncodeToPNG());
            DestroyImmediate(_texture);
            AssetDatabase.Refresh();

            texPath = string.Format("{0}/{1}.png", _projRelativeSaveFolder, _filename);
            _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);

            // Change import settings
            var importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
            importer.filterMode = FilterMode.Point;
            importer.maxTextureSize = 32;
            importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.textureType = TextureImporterType.Image;
            importer.SaveAndReimport();
        }

        private void OnWizardCreate()
        {
            CreateTexture();
            CreateMaterials();
        }

        private void UpdateValidity()
        {
            if (string.IsNullOrEmpty(_projRelativeSaveFolder))
            {
                errorString = "Must be saved in project.";
                isValid = false;
            }
            else
            {
                errorString = "";
                isValid = true;
            }
        }
    }
}
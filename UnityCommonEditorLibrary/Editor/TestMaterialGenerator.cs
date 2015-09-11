using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary {
    public class TestMaterialGenerator : ScriptableWizard {
        public static TestMaterialGenerator wizard;

        public string filename = "TestMat";
        public Color32 primary = new Color32(255, 255, 255, 255);
        public Color32 secondary = new Color32(190, 190, 190, 255);
        public List<Vector2> scales = new List<Vector2>();

        Texture2D texture;
        string projRelativePath, path;

        [MenuItem("Assets/Create/Test Materials")]
        static void CreateWizard() {
            wizard = DisplayWizard<TestMaterialGenerator>("Create Test Material", "Create");
            wizard.scales.Add(Vector2.one);
        }

        void OnWizardCreate() {
            CreateTexture();
            CreateMaterials();
        }

        private void CreateMaterials() {
            var distinct = scales.Distinct();
            foreach(var s in distinct) {
                var newMat = new Material(Shader.Find("Standard"));
                newMat.mainTexture = texture;
                newMat.mainTextureScale = s;
                AssetDatabase.CreateAsset(newMat, string.Format("Assets/{0} {1}x{2}.mat", filename, s.x, s.y));
            }
        }

        private void CreateTexture() {
            // Clamp alpha
            primary.a = secondary.a = 255;

            // Get paths
            path = string.Format("{0}/{1}.png", Application.dataPath, filename);
            projRelativePath = FileUtil.GetProjectRelativePath(path);

            // Create texture
            texture = new Texture2D(2, 2);
            texture.filterMode = FilterMode.Point;
            texture.SetPixels32(new Color32[] { primary, secondary, secondary, primary });
            texture.Apply();

            // Write to path
            File.WriteAllBytes(path, texture.EncodeToPNG());
            DestroyImmediate(texture);
            AssetDatabase.Refresh();

            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(projRelativePath);

            // Change import settings
            var importer = AssetImporter.GetAtPath(projRelativePath) as TextureImporter;
            importer.filterMode = FilterMode.Point;
            importer.maxTextureSize = 32;
            importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.textureType = TextureImporterType.Image;
            importer.SaveAndReimport();
        }

    }
}

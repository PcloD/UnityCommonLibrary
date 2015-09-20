using UnityEditor;

namespace UnityCommonEditorLibrary {
    [InitializeOnLoad]
    public class ReadOnlyAttributeToggler {

        [PreferenceItem("Misc.")]
        static void ReadOnlyPreferences() {
            if(!EditorPrefs.HasKey("ShowReadOnlyFields")) {
                EditorPrefs.SetBool("ShowReadOnlyFields", true);
            }

            EditorPrefs.SetBool("ShowReadOnlyFields", EditorGUILayout.Toggle("Show ReadOnly Fields", EditorPrefs.GetBool("ShowReadOnlyFields")));
        }

    }
}
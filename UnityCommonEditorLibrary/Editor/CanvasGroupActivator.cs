using UnityEditor;
using UnityEngine;

namespace UnityCommonEditorLibrary
{
    public class CanvasGroupActivator : EditorWindow
    {

        [MenuItem("Window/Tools/Canvas Groups Activator")]
        private static void InitWindow()
        {
            GetWindow<CanvasGroupActivator>();
        }

        private CanvasGroup[] canvasGroups;

        private void OnEnable()
        {
            ObtainCanvasGroups();
        }

        private void OnFocus()
        {
            ObtainCanvasGroups();
        }

        private void ObtainCanvasGroups()
        {
            canvasGroups = GameObject.FindObjectsOfType<CanvasGroup>();
        }

        private void OnGUI()
        {
            if (canvasGroups == null)
            {
                return;
            }

            GUILayout.Space(10f);
            GUILayout.Label("Canvas Groups");

            for (int i = 0; i < canvasGroups.Length; i++)
            {
                if (canvasGroups[i] == null)
                {
                    continue;
                }

                bool initialActive = false;
                if (canvasGroups[i].alpha == 1.0f)
                {
                    initialActive = true;
                }

                bool active = EditorGUILayout.Toggle(canvasGroups[i].name, initialActive);
                if (active != initialActive)
                {
                    //If deactivated and initially active
                    if (!active && initialActive)
                    {
                        //Deactivate this
                        canvasGroups[i].alpha = 0f;
                        canvasGroups[i].interactable = false;
                        canvasGroups[i].blocksRaycasts = false;
                    }
                    //If activated and initially deactive
                    else if (active && !initialActive)
                    {
                        //Deactivate all others and activate this
                        HideAllGroups();

                        canvasGroups[i].alpha = 1.0f;
                        canvasGroups[i].interactable = true;
                        canvasGroups[i].blocksRaycasts = true;
                    }
                }
            }

            GUILayout.Space(5f);

            if (GUILayout.Button("Show All"))
            {
                ShowAllGroups();
            }

            if (GUILayout.Button("Hide All"))
            {
                HideAllGroups();
            }
        }

        private void ShowAllGroups()
        {
            foreach (var cg in canvasGroups)
            {
                if (cg != null)
                {
                    cg.alpha = 1.0f;
                    cg.interactable = true;
                    cg.blocksRaycasts = true;
                }
            }
        }

        private void HideAllGroups()
        {
            foreach (var cg in canvasGroups)
            {
                if (cg != null)
                {
                    cg.alpha = 0.0f;
                    cg.interactable = false;
                    cg.blocksRaycasts = false;
                }
            }
        }
    }
}
using UnityEngine;
using UEGFX = UnityEngine.Graphics;

// fixes the deferred lighting missing final copy&resolve, so the next camera gets the correctly final processed image in the temp screen RT as input
// NOTE: The script must be the last in the image effect chain, so order it in the inspector!
namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public class CopyToScreenRT : UCScript {
        private RenderTexture activeRT; // hold the org. screen RT

        void OnPreRender() {
            if(GetComponent<Camera>().renderingPath == RenderingPath.DeferredShading) {
                activeRT = RenderTexture.active;
            }
            else {
                activeRT = null;
            }
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if(GetComponent<Camera>().renderingPath == RenderingPath.DeferredShading && activeRT) {
                if(src.format == activeRT.format) {
                    UEGFX.Blit(src, activeRT);
                }
                else {
                    Debug.LogWarning("Cant resolve texture, because of different formats!");
                }
            }

            // script must be last anyway, so we don't need a final copy?
            UEGFX.Blit(src, dest); // just in case we are not last!
        }
    }
}
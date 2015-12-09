using UnityEngine;

namespace UnityCommonLibrary {
    [ExecuteInEditMode]
    public class ParallaxLayer : MonoBehaviour {
        new public ParallaxCamera camera;

        [SerializeField]
        float speedX, speedY;

        [SerializeField]
        bool reverseDirection;

        Vector3 prevCamPosition;
        bool prevMoveParallax;

        void OnEnable() {
            if(camera == null) {
                camera = FindObjectOfType<ParallaxCamera>();
            }
            if(camera != null) {
                prevCamPosition = camera.transform.position;
            }
        }

        void Update() {
            if(camera == null) {
                return;
            }

            if(camera.moveParallax && !prevMoveParallax) {
                prevCamPosition = camera.transform.position;
            }

            prevMoveParallax = camera.moveParallax;

            if(!Application.isPlaying && !camera.moveParallax) {
                return;
            }

            var dist = camera.transform.position - prevCamPosition;
            var dir = (reverseDirection) ? -1f : 1f;
            transform.position += Vector3.Scale(dist, new Vector3(speedX, speedY)) * dir;

            prevCamPosition = camera.transform.position;
        }
    }
}
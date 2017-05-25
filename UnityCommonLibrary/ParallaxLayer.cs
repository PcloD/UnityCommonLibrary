using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    public class ParallaxLayer : MonoBehaviour
    {
        public ParallaxCamera Camera;
        public bool ReverseDirection;
        public float SpeedX;
        public float SpeedY;

        private Vector3 _prevCamPosition;
        private bool _prevMoveParallax;

        private void OnEnable()
        {
            if (Camera == null)
            {
                Camera = FindObjectOfType<ParallaxCamera>();
            }
            if (Camera != null)
            {
                _prevCamPosition = Camera.transform.position;
            }
        }

        private void Update()
        {
            if (!Camera)
            {
                return;
            }
            if (Camera.MoveParallax && !_prevMoveParallax)
            {
                _prevCamPosition = Camera.transform.position;
            }
            _prevMoveParallax = Camera.MoveParallax;
            if (!Application.isPlaying && !Camera.MoveParallax)
            {
                return;
            }
            var dist = Camera.transform.position - _prevCamPosition;
            var dir = ReverseDirection ? -1f : 1f;
            transform.position += Vector3.Scale(dist, new Vector3(SpeedX, SpeedY)) * dir;

            _prevCamPosition = Camera.transform.position;
        }
    }
}
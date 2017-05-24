using UnityEngine;

namespace UnityCommonLibrary
{
    public class ParallaxCamera : MonoBehaviour
    {
        public bool MoveParallax;

        [SerializeField]
        [HideInInspector]
        private Vector3 _storedPosition;

        public void RestorePosition()
        {
            transform.position = _storedPosition;
        }

        public void SavePosition()
        {
            _storedPosition = transform.position;
        }
    }
}
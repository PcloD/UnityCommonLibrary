using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _amount = Vector3.one;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private bool _invert;

        private void Update()
        {
            var cam = _camera == null ? Camera.main : _camera;
            if (!cam)
            {
                return;
            }
            transform.LookAt(cam.transform);
            if (_invert)
            {
                transform.forward *= -1f;
            }
            transform.eulerAngles = Vector3.Scale(transform.eulerAngles, _amount);
        }
    }
}
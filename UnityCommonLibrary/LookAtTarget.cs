using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    public class LookAtTarget : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _amount = Vector3.one;
        [SerializeField]
        private Transform _target;

        private void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }
            transform.LookAt(_target);
            transform.eulerAngles = Vector3.Scale(transform.eulerAngles, _amount);
        }
    }
}
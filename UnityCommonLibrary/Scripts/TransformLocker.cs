using UnityEngine;

namespace UnityCommonLibrary
{
    [ExecuteInEditMode]
    public class TransformLocker : MonoBehaviour
    {
        private void Update()
        {
            transform.hideFlags = HideFlags.HideInInspector;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
        private void OnDestroy()
        {
            transform.hideFlags = HideFlags.None;
        }
    }
}
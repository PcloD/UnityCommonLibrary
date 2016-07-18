using UnityEngine;

namespace UnityCommonLibrary
{
    [DisallowMultipleComponent]
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}

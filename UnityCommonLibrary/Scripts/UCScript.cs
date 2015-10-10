using System;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityCommonLibrary {
    [Serializable]
    public abstract class UCScript : MonoBehaviour {

        public void TSCancelInvoke(Action a) {
            CancelInvoke(a.Method.Name);
        }

        public void TSInvoke(Action a, float time) {
            Invoke(a.Method.Name, time);
        }

        public void TSCancelInvoke<T>(Action<T> a) {
            CancelInvoke(a.Method.Name);
        }

    }
}
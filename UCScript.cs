using System;
using System.Collections;
using UnityEngine;

namespace UnityCommonLibrary {
    [Serializable]
    public abstract class UCScript : MonoBehaviour {

        public Coroutine Exec(IEnumerator ie) {
            return StartCoroutine(ie);
        }

        public void TSInvoke(Action a, float time) {
            Exec(_TSInvoke(a, time, false));
        }

        public void TSInvoke(Action a, float time, bool unscaled) {
            Exec(_TSInvoke(a, time, unscaled));
        }

        public void TSInvoke<T>(Action<T> a, T arg, float time) {
            Exec(_TSInvoke(a, arg, time, false));
        }

        public void TSInvoke<T>(Action<T> a, T arg, float time, bool unscaled) {
            Exec(_TSInvoke(a, arg, time, unscaled));
        }

        public void TSInvoke(IEnumerator a, float time) {
            Exec(_TSInvoke(a, time, false));
        }

        public void TSInvoke(IEnumerator a, float time, bool unscaled) {
            Exec(_TSInvoke(a, time, unscaled));
        }

        IEnumerator _TSInvoke(IEnumerator a, float time, bool unscaled) {
            if(unscaled) {
                var start = Time.unscaledTime;
                while(Time.unscaledTime - start < time) {
                    yield return null;
                }
            }
            else {
                yield return new WaitForSeconds(time);
            }
            Exec(a);
        }

        IEnumerator _TSInvoke(Action a, float time, bool unscaled) {
            if(unscaled) {
                var start = Time.unscaledTime;
                while(Time.unscaledTime - start < time) {
                    yield return null;
                }
            }
            else {
                yield return new WaitForSeconds(time);
            }
            a();
        }

        IEnumerator _TSInvoke<T>(Action<T> a, T arg, float time, bool unscaled) {
            if(unscaled) {
                var start = Time.unscaledTime;
                while(Time.unscaledTime - start < time) {
                    yield return null;
                }
            }
            else {
                yield return new WaitForSeconds(time);
            }
            a(arg);
        }
    }
}
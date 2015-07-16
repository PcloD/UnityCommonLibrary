using System;
using UnityEngine;

namespace UnityCommonLibrary {
    public class UCOSingleton<T> : UCObject where T : UCOSingleton<T> {
        private static object @lock = new object();
        private static T _get;

        public static T get {
            get {
                return GetSingleton();
            }
        }

        private static T GetSingleton() {
            lock (@lock) {
                if(_get == null) {
                    _get = Activator.CreateInstance<T>();
                    _get.OnInitialCreate();
                }
                return _get;
            }
        }

        protected virtual void OnInitialCreate() { }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary {
    /// <typeparam name="C">The type of collider to check.</typeparam>
    /// <typeparam name="I">The information to pass to the watchers.</typeparam>
    public abstract class AbstractWatcher<C, I> : MonoBehaviour {

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns>True if should be removed as listener</returns>
        public delegate bool OnEvent(I i);

        protected Dictionary<C, OnEvent> watchers = new Dictionary<C, OnEvent>();

        event OnEvent Event;

        internal void AddWatcher(C c, OnEvent e) {
            watchers.Add(c, e);
        }

        internal void RemoveWatcher(C c) {
            watchers.Remove(c);
        }

        internal void RegisterListener(OnEvent e) {
            Event += e;
        }

        internal void RemoveAllListeners() {
            if(Event != null) {
                foreach(var e in Event.GetInvocationList()) {
                    Event -= (OnEvent)e;
                }
            }
        }

        void CheckListeners(I info) {
            if(Event != null) {
                foreach(var e in Event.GetInvocationList()) {
                    OnEvent oe = (OnEvent)e;
                    if(oe(info)) {
                        Event -= oe;
                    }
                }
            }
        }

        protected void Process(C collider, I info) {
            OnEvent del = null;
            var found = watchers.TryGetValue(collider, out del);
            if(found) {
                del(info);
            }
            CheckListeners(info);
        }
    }

    #region 3D

    /// <typeparam name="I">The information to pass to the watchers.</typeparam>
    public abstract class Watcher<I> : AbstractWatcher<Collider, I> { }

    [RequireComponent(typeof(Collider))]
    public abstract class CollisionWatcher : Watcher<Collision> { }

    [RequireComponent(typeof(Collider))]
    public abstract class TriggerWatcher : Watcher<Collider> { }

    public class CollisionEnterWatcher : CollisionWatcher {

        void OnCollisionEnter(Collision c) {
            Process(c.collider, c);
        }
    }

    public class CollisionExitWatcher : CollisionWatcher {

        void OnCollisionExit(Collision c) {
            Process(c.collider, c);
        }
    }

    public class CollisionStayWatcher : CollisionWatcher {

        void OnCollisionStay(Collision c) {
            Process(c.collider, c);
        }
    }

    public class TriggerEnterWatcher : TriggerWatcher {

        void OnTriggerEnter(Collider c) {
            Process(c, c);
        }
    }

    public class TriggerExitWatcher : TriggerWatcher {

        void OnTriggerExit(Collider c) {
            Process(c, c);
        }
    }

    public class TriggerStayWatcher : TriggerWatcher {

        void OnTriggerStay(Collider c) {
            Process(c, c);
        }
    }

    #endregion 3D

    #region 2D

    /// <typeparam name="I">The information to pass to the watchers.</typeparam>
    public abstract class Watcher2D<I> : AbstractWatcher<Collider2D, I> { }

    [RequireComponent(typeof(Collider2D))]
    public abstract class CollisionWatcher2D : Watcher2D<Collision2D> { }

    [RequireComponent(typeof(Collider2D))]
    public abstract class TriggerWatcher2D : Watcher2D<Collider2D> { }

    public class CollisionEnterWatcher2D : CollisionWatcher2D {

        void OnCollisionEnter2D(Collision2D c) {
            Process(c.collider, c);
        }
    }

    public class CollisionExitWatcher2D : CollisionWatcher2D {

        void OnCollisionExit2D(Collision2D c) {
            Process(c.collider, c);
        }
    }

    public class CollisionStayWatcher2D : CollisionWatcher2D {

        void OnCollisionStay2D(Collision2D c) {
            Process(c.collider, c);
        }
    }

    public class TriggerEnterWatcher2D : TriggerWatcher2D {

        void OnTriggerEnter2D(Collider2D c) {
            Process(c, c);
        }
    }

    public class TriggerExitWatcher2D : TriggerWatcher2D {

        void OnTriggerExit2D(Collider2D c) {
            Process(c, c);
        }
    }

    public class TriggerStayWatcher2D : TriggerWatcher2D {

        void OnTriggerStay2D(Collider2D c) {
            Process(c, c);
        }
    }

    #endregion 2D

    public static class ColliderExts {

        //Add Watcher3D
        static void ListenTrigger<T>(Collider collider, Collider c, TriggerWatcher.OnEvent e) where T : TriggerWatcher {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.AddWatcher(c, e);
        }

        static void ListenCollision<T>(Collider collider, Collider c, CollisionWatcher.OnEvent e) where T : CollisionWatcher {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.AddWatcher(c, e);
        }

        //Add Watcher2D
        static void ListenTrigger<T>(Collider2D collider, Collider2D c, TriggerWatcher2D.OnEvent e) where T : TriggerWatcher2D {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.AddWatcher(c, e);
        }

        static void ListenCollision<T>(Collider2D collider, Collider2D c, CollisionWatcher2D.OnEvent e) where T : CollisionWatcher2D {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.AddWatcher(c, e);
        }

        //Add Listener3D
        static void ListenTrigger<T>(Collider collider, TriggerWatcher.OnEvent e) where T : TriggerWatcher {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.RegisterListener(e);
        }

        static void ListenCollision<T>(Collider collider, CollisionWatcher.OnEvent e) where T : CollisionWatcher {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.RegisterListener(e);
        }

        //Add Listener2D
        static void ListenTrigger<T>(Collider2D collider, TriggerWatcher2D.OnEvent e) where T : TriggerWatcher2D {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.RegisterListener(e);
        }

        static void ListenCollision<T>(Collider2D collider, CollisionWatcher2D.OnEvent e) where T : CollisionWatcher2D {
            var watcher = collider.GetOrAddComponent<T>();
            watcher.RegisterListener(e);
        }

        //Add TriggerWatcher3D
        public static void ListenTriggerEnter(this Collider collider, Collider c, TriggerWatcher.OnEvent e) {
            ListenTrigger<TriggerEnterWatcher>(collider, c, e);
        }

        public static void ListenTriggerExit(this Collider collider, Collider c, TriggerWatcher.OnEvent e) {
            ListenTrigger<TriggerExitWatcher>(collider, c, e);
        }

        public static void ListenTriggerStay(this Collider collider, Collider c, TriggerWatcher.OnEvent e) {
            ListenTrigger<TriggerStayWatcher>(collider, c, e);
        }

        //Add CollisionWatcher3D
        public static void ListenCollisionEnter(this Collider collider, Collider c, CollisionWatcher.OnEvent e) {
            ListenCollision<CollisionEnterWatcher>(collider, c, e);
        }

        public static void ListenCollisionExit(this Collider collider, Collider c, CollisionWatcher.OnEvent e) {
            ListenCollision<CollisionExitWatcher>(collider, c, e);
        }

        public static void ListenCollisionStay(this Collider collider, Collider c, CollisionWatcher.OnEvent e) {
            ListenCollision<CollisionStayWatcher>(collider, c, e);
        }

        //Add TriggerWatcher2D
        public static void ListenTriggerEnter(this Collider2D collider, Collider2D c, TriggerWatcher2D.OnEvent e) {
            ListenTrigger<TriggerEnterWatcher2D>(collider, c, e);
        }

        public static void ListenTriggerExit(this Collider2D collider, Collider2D c, TriggerWatcher2D.OnEvent e) {
            ListenTrigger<TriggerExitWatcher2D>(collider, c, e);
        }

        public static void ListenTriggerStay(this Collider2D collider, Collider2D c, TriggerWatcher2D.OnEvent e) {
            ListenTrigger<TriggerStayWatcher2D>(collider, c, e);
        }

        //Add CollisionWatcher2D
        public static void ListenCollisionEnter(this Collider2D collider, Collider2D c, CollisionWatcher2D.OnEvent e) {
            ListenCollision<CollisionEnterWatcher2D>(collider, c, e);
        }

        public static void ListenCollisionExit(this Collider2D collider, Collider2D c, CollisionWatcher2D.OnEvent e) {
            ListenCollision<CollisionExitWatcher2D>(collider, c, e);
        }

        public static void ListenCollisionStay(this Collider2D collider, Collider2D c, CollisionWatcher2D.OnEvent e) {
            ListenCollision<CollisionStayWatcher2D>(collider, c, e);
        }

        //Add TriggerListener3D
        public static void ListenTriggerEnter(this Collider collider, TriggerWatcher.OnEvent e) {
            ListenTrigger<TriggerEnterWatcher>(collider, e);
        }

        public static void ListenTriggerExit(this Collider collider, TriggerWatcher.OnEvent e) {
            ListenTrigger<TriggerExitWatcher>(collider, e);
        }

        public static void ListenTriggerStay(this Collider collider, TriggerWatcher.OnEvent e) {
            ListenTrigger<TriggerStayWatcher>(collider, e);
        }

        //Add CollisionListener3D
        public static void ListenCollisionEnter(this Collider collider, CollisionWatcher.OnEvent e) {
            ListenCollision<CollisionEnterWatcher>(collider, e);
        }

        public static void ListenCollisionExit(this Collider collider, CollisionWatcher.OnEvent e) {
            ListenCollision<CollisionExitWatcher>(collider, e);
        }

        public static void ListenCollisionStay(this Collider collider, CollisionWatcher.OnEvent e) {
            ListenCollision<CollisionStayWatcher>(collider, e);
        }

        //Add TriggerListener2D
        public static void ListenTriggerEnter(this Collider2D collider, TriggerWatcher2D.OnEvent e) {
            ListenTrigger<TriggerEnterWatcher2D>(collider, e);
        }

        public static void ListenTriggerExit(this Collider2D collider, TriggerWatcher2D.OnEvent e) {
            ListenTrigger<TriggerExitWatcher2D>(collider, e);
        }

        public static void ListenTriggerStay(this Collider2D collider, TriggerWatcher2D.OnEvent e) {
            ListenTrigger<TriggerStayWatcher2D>(collider, e);
        }

        //Add CollisionListener2D
        public static void ListenCollisionEnter(this Collider2D collider, CollisionWatcher2D.OnEvent e) {
            ListenCollision<CollisionEnterWatcher2D>(collider, e);
        }

        public static void ListenCollisionExit(this Collider2D collider, CollisionWatcher2D.OnEvent e) {
            ListenCollision<CollisionExitWatcher2D>(collider, e);
        }

        public static void ListenCollisionStay(this Collider2D collider, CollisionWatcher2D.OnEvent e) {
            ListenCollision<CollisionStayWatcher2D>(collider, e);
        }

        static T GetOrAddComponent<T>(this Component c) where T : Component {
            var watcher = c.GetComponent<T>();
            if(watcher == null) {
                watcher = c.gameObject.AddComponent<T>();
            }
            return watcher;
        }
    }
}
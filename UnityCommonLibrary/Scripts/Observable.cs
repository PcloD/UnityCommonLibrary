using System.Collections.Generic;
using System.Linq;

namespace UnityCommonLibrary {
    /// <summary>
    /// An object that can be subscribed to upon notification.
    /// </summary>
    /// <typeparam name="O">Self reference restraint.</typeparam>
    public abstract class Observable<O> where O : Observable<O> {
        static HashSet<IObserver<O>> observers = new HashSet<IObserver<O>>();

        /// <summary>
        /// Subscribe an IObserver to this observable.
        /// This subscriber will always respond to a notification.
        /// </summary>
        /// <param name="observer">The IObserver to subscribe.</param>
        public static void Subscribe(IObserver<O> observer) {
            observers.Add(observer);
        }

        /// <summary>
        /// Remove an IObserver from the subscription list.
        /// Called pre-subscribe.
        /// </summary>
        /// <param name="observer">The IObserver to unsubscribe</param>
        public static void Unsubscribe(IObserver<O> observer) {
            observers.Remove(observer);
        }

        /// <summary>
        /// Notify all subscribers.
        /// </summary>
        public static void Notify() {
            foreach(var o in observers) {
                o.OnNotify();
            }
        }

    }

    /// <summary>
    /// An observable that has a single argument when notifying.
    /// </summary>
    /// <typeparam name="O">Self reference restraint.</typeparam>
    /// <typeparam name="T">Argument type</typeparam>
    public abstract class Observable<O, T> where O : Observable<O, T> {
        static HashSet<IObserver<O, T>> observers = new HashSet<IObserver<O, T>>();
        static Dictionary<IObserver<O, T>, T[]> restrictedObservers = new Dictionary<IObserver<O, T>, T[]>();

        /// <summary>
        /// Subscribe an IObserver to this observable.
        /// This subscriber will always respond to a notification.
        /// </summary>
        /// <param name="observer">The IObserver to subscribe.</param>
        public static void Subscribe(IObserver<O, T> observer) {
            Unsubscribe(observer);
            observers.Add(observer);
        }

        /// <summary>
        /// Subscribe an IObserver to this observable.
        /// IObserver is notified only when notification's arg is one of restrictions.
        /// </summary>
        /// <param name="observer">The IObserver to subscribe.</param>
        /// <param name="restrictions">A list of args to wait for before responding to notification.</param>
        public static void Subscribe(IObserver<O, T> observer, params T[] restrictions) {
            Unsubscribe(observer);
            restrictedObservers.Add(observer, restrictions);
        }

        /// <summary>
        /// Remove an IObserver from the subscription list.
        /// Called pre-subscribe.
        /// </summary>
        /// <param name="observer">The IObserver to unsubscribe</param>
        public static void Unsubscribe(IObserver<O, T> observer) {
            restrictedObservers.Remove(observer);
            observers.Remove(observer);
        }

        /// <summary>
        /// Notify all restricted subscribers if arg is contained in their restriction list.
        /// Always notifys all non-restricted subscribers.
        /// </summary>
        /// <param name="arg">Required arg to pass to subscribers</param>
        public static void Notify(T arg) {
            foreach(var o in restrictedObservers.Where(o => o.Value.Contains(arg))) {
                o.Key.OnNotify(arg);
            }
            foreach(var o in observers) {
                o.OnNotify(arg);
            }
        }

    }
}

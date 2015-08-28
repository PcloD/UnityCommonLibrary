using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityCommonLibrary {
    /// <summary>
    /// An object that can be subscribed to upon notification.
    /// </summary>
    /// <typeparam name="O">Self reference restraint.</typeparam>
    public abstract class Observable<O> where O : Observable<O> {
        public static HashSet<IObserver<O>> observers = new HashSet<IObserver<O>>();
        public static bool hasObservers { get { return observers.Count > 0; } }
        public static List<IObserver<O>> toRemove = new List<IObserver<O>>();

        /// <summary>
        /// Subscribe an IObserver to this observable.
        /// This subscriber will always respond to a notification.
        /// </summary>
        /// <param name="observer">The IObserver to subscribe.</param>
        public static void Subscribe(IObserver<O> observer) {
            if(!observers.Contains(observer)) {
                observers.Add(observer);
            }
        }

        /// <summary>
        /// Remove an IObserver from the subscription list.
        /// Called pre-subscribe.
        /// </summary>
        /// <param name="observer">The IObserver to unsubscribe</param>
        public static void Unsubscribe(IObserver<O> observer) {
            toRemove.Add(observer);
        }

        /// <summary>
        /// Notify all subscribers.
        /// </summary>
        public static void Notify(object source) {
            //pre-pass remove
            foreach(var o in toRemove) {
                observers.Remove(o);
            }
            toRemove.Clear();
            foreach(var o in observers) {
                if(o == null || (o is UnityEngine.Object && !(o as UnityEngine.Object))) {
                    toRemove.Add(o);
                }
                else if(o != source) {
                    o.OnNotify();
                }
            }

            //post-pass remove
            foreach(var o in toRemove) {
                observers.Remove(o);
            }
            toRemove.Clear();
        }

    }

    /// <summary>
    /// An observable that has a single argument when notifying.
    /// </summary>
    /// <typeparam name="O">Self reference restraint.</typeparam>
    /// <typeparam name="T">Argument type</typeparam>
    public abstract class Observable<O, T> where O : Observable<O, T> {
        public delegate bool OnRestrictNotification(T arg);

        public static HashSet<IObserver<O, T>> observers = new HashSet<IObserver<O, T>>();
        public static Dictionary<IObserver<O, T>, OnRestrictNotification> restrictedObservers = new Dictionary<IObserver<O, T>, OnRestrictNotification>();
        public static Dictionary<IObserver<O, T>, T[]> filteredObservers = new Dictionary<IObserver<O, T>, T[]>();
        public static bool hasObservers { get { return observers.Count + restrictedObservers.Count + filteredObservers.Count > 0; } }
        public static List<IObserver<O, T>> toRemove = new List<IObserver<O, T>>();


        public static bool HasObserverForFilter(T filter) {
            return filteredObservers.Any(p => p.Value.Any(v => v.Equals(filter)));
        }

        /// <summary>
        /// Subscribe an IObserver to this observable.
        /// This subscriber will always respond to a notification.
        /// </summary>
        /// <param name="observer">The IObserver to subscribe.</param>
        public static void Subscribe(IObserver<O, T> observer) {
            if(!observers.Contains(observer)) {
                observers.Add(observer);
            }
        }

        /// <summary>
        /// Subscribe an IObserver to this observable.
        /// IObserver is notified only when notification's arg is one of restrictions.
        /// </summary>
        /// <param name="observer">The IObserver to subscribe.</param>
        /// <param name="restriction">A list of args to wait for before responding to notification.</param>
        public static void Subscribe(IObserver<O, T> observer, OnRestrictNotification restriction) {
            restrictedObservers[observer] = restriction;
        }

        /// <summary>
        /// Subscribe an IObserver to this observable.
        /// IObserver is notified only when notification's arg is one of restrictions.
        /// </summary>
        /// <param name="observer">The IObserver to subscribe.</param>
        /// <param name="restrictions">A list of args to wait for before responding to notification.</param>
        public static void Subscribe(IObserver<O, T> observer, params T[] filters) {
            filteredObservers[observer] = filters;
        }

        /// <summary>
        /// Remove an IObserver from the subscription list.
        /// Called pre-subscribe.
        /// </summary>
        /// <param name="observer">The IObserver to unsubscribe</param>
        public static void Unsubscribe(IObserver<O, T> observer) {
            toRemove.Add(observer);
        }

        /// <summary>
        /// Notify all restricted subscribers if arg is contained in their restriction list.
        /// Always notifys all non-restricted subscribers.
        /// </summary>
        /// <param name="arg">Required arg to pass to subscribers</param>
        public static void Notify(object source, T arg) {
            //pre-pass remove
            foreach(var o in toRemove) {
                RemoveFromCollections(o);
            }
            toRemove.Clear();

            foreach(var o in restrictedObservers) {
                if(!o.Value(arg)) {
                    continue;
                }
                if(o.Key == null || (o.Key is UnityEngine.Object && !(o.Key as UnityEngine.Object))) {
                    toRemove.Add(o.Key);
                }
                else if(o.Key != source) {
                    o.Key.OnNotify(arg);
                }
            }

            foreach(var o in filteredObservers.Where(p => p.Value.Any(v => v.Equals(arg)))) {
                if(o.Key == null || (o.Key is UnityEngine.Object && !(o.Key as UnityEngine.Object))) {
                    toRemove.Add(o.Key);
                }
                else if(o.Key != source) {
                    o.Key.OnNotify(arg);
                }
            }

            foreach(var o in observers) {
                if(o == null || (o is UnityEngine.Object && !(o as UnityEngine.Object))) {
                    toRemove.Add(o);
                }
                else if(o != source) {
                    o.OnNotify(arg);
                }
            }

            //pre-pass remove
            foreach(var o in toRemove) {
                RemoveFromCollections(o);
            }
            toRemove.Clear();
        }

        static void RemoveFromCollections(IObserver<O, T> o) {
            observers.Remove(o);
            restrictedObservers.Remove(o);
            filteredObservers.Remove(o);
        }
    }
}

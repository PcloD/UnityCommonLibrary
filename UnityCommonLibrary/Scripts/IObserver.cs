namespace UnityCommonLibrary {

    /// <summary>
    /// An object that can be subscribed to an Observable of type O.
    /// Will always respond to notifications. Cannot receive arguments.
    /// </summary>
    /// <typeparam name="O">The Observable to subscribe to.</typeparam>
    public interface IObserver<O> where O : Observable<O> {
        void OnNotify();
    }

    /// <summary>
    /// An object that can be subscribed to an Observable of type O
    /// that allows passing of an argument of type T.
    /// Can be restricted upon subscription to only respond to notifications passing
    /// specific arguments of type T.
    /// </summary>
    /// <typeparam name="O">The Observable to subscribe to.</typeparam>
    /// <typeparam name="T">The argument type to use. Must match Observable's T constraint.</typeparam>
    public interface IObserver<O, T> where O : Observable<O, T> {
        void OnNotify(T arg);
    }

}
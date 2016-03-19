using System;

namespace UnityCommonLibrary.Utilities {
    public delegate void GameEvent();
    public delegate void GameEvent<S>(S source);
    public delegate void GameEvent<S, T>(S source, T args) where T : EventArgs;
}

namespace UnityCommonLibrary.Input {
    [System.Flags]
    public enum ControlState {
        None = 0,
        Held = 1 << 0,
        Down = 1 << 1,
        Up = 1 << 2,
        All = Held | Down | Up
    }
}
using System;

namespace UnityCommonLibrary {
    [Flags]
    public enum TransformElement {
        None = 0,
        Position = 1 << 0,
        Rotation = 1 << 1,
        Scale = 1 << 2,
        All = Position | Rotation | Scale
    }
}

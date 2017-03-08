using System;

namespace UnityCommonLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class NullProviderAttribute : Attribute
    {
        public NullProviderAttribute() { }
    }
}

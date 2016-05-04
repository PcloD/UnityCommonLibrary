using System;

namespace UnityCommonLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AutoInstantiateAttribute : Attribute
    {
        public AutoInstantiateAttribute() { }
    }
}

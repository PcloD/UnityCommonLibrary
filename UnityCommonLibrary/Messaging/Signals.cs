using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
    public static class Signals
    {
        internal static readonly HashSet<ISignal> signals = new HashSet<ISignal>();

        public static void UnsubscribeFromAll(object target)
        {
            foreach (var s in signals)
            {
                s.UnsubscribeTarget(target);
            }
        }
    }
}

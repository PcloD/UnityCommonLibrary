using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
    public static class Signals
    {
        internal static readonly HashSet<ISignal> AllSignals = new HashSet<ISignal>();

        public static void UnsubscribeFromAll(object target)
        {
            foreach (var s in AllSignals)
            {
                s.UnsubscribeTarget(target);
            }
        }
    }
}
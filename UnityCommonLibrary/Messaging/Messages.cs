using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
    public static class Messages
    {
        internal static readonly HashSet<IMessage> AllSignals = new HashSet<IMessage>();

        public static void UnsubscribeFromAll(object target)
        {
            foreach (var s in AllSignals)
            {
                s.UnsubscribeTarget(target);
            }
        }
    }
}
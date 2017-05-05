using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityCommonLibrary.Messaging
{
    public static class Events
    {
        internal static readonly HashSet<IEvent> events = new HashSet<IEvent>();

        public static void Update()
        {
            foreach (var evt in events)
            {
                evt.Update();
            }
        }
        public static void UnsubscribeFromAll(object target)
        {
            foreach (var evt in events)
            {
                evt.UnsubscribeTarget(target);
            }
        }
        internal static bool CheckRemoveCallback(Delegate d)
        {
            return d.Target == null && !d.Method.IsStatic;
        }
    }
}

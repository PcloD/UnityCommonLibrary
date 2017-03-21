using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
    public abstract class MessageData
    {
        public virtual void PrepareBroadcast() { }
    }
}

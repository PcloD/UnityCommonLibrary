using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
    public sealed class GenericMessageData<T> : MessageData
    {
        public T value { get; private set; }

        public GenericMessageData(T value)
        {
            this.value = value;
        }
    }
    public sealed class CollectionMessageData : MessageData
    {
        internal bool isLocked;
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();

        public CollectionMessageData(string key, object value)
        {
            data.Add(key, value);
        }

        public bool TryGetData<T>(string key, out T value)
        {
            try
            {
                value = GetData<T>(key);
                return true;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }
        public T GetData<T>(string key)
        {
            return (T)data[key];
        }
        public MessageData Append(string key, object value)
        {
            if (!isLocked)
            {
                data.Add(key, value);
            }
            return this;
        }
        public void OnBroadcast()
        {
            isLocked = true;
        }
    }
}

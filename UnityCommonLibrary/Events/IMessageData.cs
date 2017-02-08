using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Messaging
{
	public interface IMessageData
	{
		void OnBroadcast();
	}
	public class GenericMessageData : IMessageData
	{
		internal bool isLocked;
		private readonly Dictionary<string, object> data = new Dictionary<string, object>();

		public GenericMessageData(string key, object value)
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
			catch(Exception)
			{
				value = default(T);
				return false;
			}
		}
		public T GetData<T>(string key)
		{
			return (T)data[key];
		}
		public IMessageData Append(string key, object value)
		{
			if(!isLocked)
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

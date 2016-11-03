using System;
using System.Collections.Generic;

namespace UnityCommonLibrary.Events
{
	public class EventData
	{
		internal bool isLocked;
		private readonly Dictionary<string, object> data = new Dictionary<string, object>();

		public EventData(string key, object value)
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
		public EventData Append(string key, object value)
		{
			if (!isLocked)
			{
				data.Add(key, value);
			}
			return this;
		}
	}
}

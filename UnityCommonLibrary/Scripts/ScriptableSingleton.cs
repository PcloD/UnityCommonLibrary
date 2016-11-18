using UnityEngine;

namespace UnityCommonLibrary
{
	public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
	{
		private static bool isShuttingDown;
		private static T _get;

		public static T get
		{
			get
			{
				if(isShuttingDown)
				{
					return null;
				}
				if(!_get)
				{
					_get = CreateInstance<T>();
				}
				return _get;
			}
		}
		public static void EnsureExists()
		{
			if(!_get)
			{
				_get = CreateInstance<T>();
			}
		}
	}
}
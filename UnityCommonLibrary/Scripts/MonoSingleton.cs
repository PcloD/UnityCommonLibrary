using UnityCommonLibrary.Utility;
using UnityEngine;

namespace UnityCommonLibrary
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
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
					GetOrCreate();
				}
				return _get;
			}
		}
		public static void EnsureExists()
		{
			if(!_get)
			{
				GetOrCreate();
			}
		}
		private static void GetOrCreate()
		{
			var all = FindObjectsOfType<T>();
			if(all.Length == 0)
			{
				_get = ComponentUtility.Create<T>();
			}
			else
			{
				_get = all[0];
			}
			DontDestroyOnLoad(_get);
			if(all.Length > 1)
			{
				Debug.LogError(string.Format("FindObjectsOfType<{0}>().Length == {1}", typeof(T).Name, all.Length));
			}
		}

		protected virtual void Awake()
		{
			DontDestroyOnLoad(this);
			if(!_get)
			{
				_get = (T)this;
			}
		}
		protected virtual void OnApplicationQuit()
		{
			isShuttingDown = true;
		}
	}

	public abstract class Singleton<T> where T : Singleton<T>, new()
	{
		public static bool usesActivator;

		private static T _get;
		public static T get
		{
			get
			{
				if(_get == null)
				{
					_get = usesActivator ? System.Activator.CreateInstance<T>() : new T();
				}
				return _get;
			}
		}
	}
}
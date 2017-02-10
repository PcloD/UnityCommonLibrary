using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCommonLibrary
{
	public static class Services
	{
		private static readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

		public static S Get<S>() where S : class
		{
			return (S)registry[typeof(S)];
		}
		public static S Register<S, P>() where S : class where P : S, new()
		{
			return Register<S>(new P());
		}
		public static S RegisterAsBehaviour<S, P>(bool dontDestroy = true) where S : class where P : MonoBehaviour, S
		{
			var provider = new GameObject(typeof(P).Name).AddComponent<P>();
			UnityEngine.Object.DontDestroyOnLoad(provider);
			return Register<S>(provider);
		}
		public static S RegisterAsScriptable<S, P>(bool dontDestroy = true) where S : class where P : ScriptableObject, S
		{
			var provider = ScriptableObject.CreateInstance<P>();
			UnityEngine.Object.DontDestroyOnLoad(provider);
			return Register<S>(provider);
		}
		public static S Register<S>(S provider) where S : class
		{
			return (S)Register(typeof(S), provider);
		}
		private static object Register(Type type, object provider)
		{
			if(registry.ContainsKey(type))
			{
				registry[type] = provider;
			}
			else
			{
				registry.Add(type, provider);
			}
			return provider;
		}
	}
}

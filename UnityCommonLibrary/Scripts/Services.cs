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
		public static S Register<S>(S instance) where S : class
		{
			registry.Add(typeof(S), instance);
			return instance;
		}
	}
}

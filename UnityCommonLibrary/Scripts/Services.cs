using System;
using System.Collections.Generic;

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
		public static S Register<S>(S instance) where S : class
		{
			registry.Add(typeof(S), instance);
			return instance;
		}
	}
}

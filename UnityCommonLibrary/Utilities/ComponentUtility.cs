using System;
using UnityEngine;

namespace UnityCommonLibrary.Utilities {
	public static class ComponentUtility {

		public static T Create<T>() where T : Component {
			return Create<T>(typeof(T).Name);
		}

		public static T Create<T>(string name) where T : Component {
			return new GameObject(name).AddComponent<T>();
		}

		public static Component Create(Type t) {
			return Create(t, t.Name);
		}

		private static Component Create(Type t, string name) {
			return new GameObject(name).AddComponent(t);
		}

		public static void Toggle(bool enabled, params Behaviour[] behaviors) {
			foreach(var b in behaviors) {
				if(b != null) {
					b.enabled = enabled;
				}
			}
		}

	}
}
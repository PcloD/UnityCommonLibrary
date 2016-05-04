using System;
using System.Linq;
using UnityCommonLibrary.Attributes;
using UnityCommonLibrary.Utilities;
using UnityEngine;

namespace UnityCommonLibrary.Scripts
{
    public class AutoInstantiator {

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Load() {
			var all = Resources.LoadAll<GameObject>("AutoInstantiator");
			foreach(var obj in all) {
				UnityEngine.Object.Instantiate(obj);
			}

			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach(var a in assemblies) {
				var types = from t in a.GetTypes()
							where t.IsClass &&
							!t.IsAbstract &&
							t.GetCustomAttributes(typeof(AutoInstantiateAttribute), true).Length > 0 &&
							typeof(Component).IsAssignableFrom(t)
							select t;

				foreach(var t in types) {
					ComponentUtility.Create(t);
				}
			}
		}

	}
}

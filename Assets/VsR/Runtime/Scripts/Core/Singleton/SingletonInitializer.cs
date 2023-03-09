using System;
using System.Linq;
using UnityEngine;

namespace VsR {
	public static class SingletonInitializer {
		[RuntimeInitializeOnLoadMethod]
		private static void Init() {
			var singletonTypes = typeof(SingletonInitializer).Assembly.GetTypes()
				.Where(t => t.BaseType != null
					&& t.BaseType.IsGenericType
					&& t.BaseType.GetGenericTypeDefinition() == typeof(SingletonBehaviour<>));

			foreach (Type type in singletonTypes)
				new GameObject(type.Name, type);
		}
	}
}

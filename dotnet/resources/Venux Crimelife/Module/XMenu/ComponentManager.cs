using System;
using System.Collections.Generic;

namespace Venux
{
	public class ComponentManager
	{
		private readonly Dictionary<Type, object> components = new Dictionary<Type, object>();

		public static ComponentManager Instance { get; } = new ComponentManager();


		private ComponentManager()
		{
		}

		public void Register(Component component)
		{
			components[((object)component).GetType()] = component;
		}

		public static T Get<T>() where T : Component
		{
			if (!Instance.components.TryGetValue(typeof(T), out var value))
			{
				return null;
			}
			return value as T;
		}
	}
}

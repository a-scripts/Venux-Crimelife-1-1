using GTANetworkAPI;

namespace Venux
{
	public abstract class Component : Script
	{
		public string Name { get; }

		public Component(string name)
		{
			Name = name;
			ComponentManager.Instance.Register(this);
		}

		public void TriggerEvent(Player player, string eventName, params object[] args)
		{
			object[] array = new object[2 + args.Length];
			array[0] = Name;
			array[1] = eventName;
			for (int i = 0; i < args.Length; i++)
			{
				array[i + 2] = args[i];
			}
			player.TriggerEvent("componentServerEvent", array);
		}
	}
}

using System;
using GTANetworkAPI;
using Newtonsoft.Json;
namespace Venux
{
	public abstract class Window<T> : Component
	{
		public class Event
		{
			[JsonIgnore]
			public DbPlayer DbPlayer { get; }

			public Event(DbPlayer dbPlayer)
			{
				DbPlayer = dbPlayer;
			}
		}

		public Window(string name)
			: base(name)
		{
		}

		public bool OnShow(Event @event)
		{
			string text;
			try
			{
				text = NAPI.Util.ToJson((object)@event);
			}
			catch (Exception ex)
			{
				text = null;
			}
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			Open(@event.DbPlayer.Client, text);
			return true;
		}

		public virtual void Open(Player player, string json)
		{
			player.TriggerEvent("openWindow", new object[2] { base.Name, json });
		}

		public virtual void Close(Player player)
		{
			player.TriggerEvent("closeWindow", new object[1] { base.Name });
		}

		public abstract T Show();
	}
}

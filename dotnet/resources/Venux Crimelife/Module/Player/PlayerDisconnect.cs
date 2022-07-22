using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    class PlayerDisconnect : Script
    {
		[ServerEvent(Event.PlayerDisconnected)]
		public void OnPlayerDisconnect(Player c, DisconnectionType type, string reason)
		{
			Integration.DiscordIntegration.UpdateStatus("" + NAPI.Pools.GetAllPlayers().Count + " Spieler auf dem Server", Discord.ActivityType.Playing, Discord.UserStatus.DoNotDisturb).ConfigureAwait(true);
			lock (c)
			{
				NAPI.Player.GetPlayersInRadiusOfPlayer(50.0, c).ForEach(delegate (Player player)
				{
					DbPlayer player3 = player.GetPlayer();
					if (player3 != null)
					{
						player3.SendNotification("Der Spieler " + c.Name + " nun Offline! (Grund: Disconnect)", 4000, "yellow", "ANTI-OFFLINEFLUCHT");
					}
				});
			}
		}
	}
}

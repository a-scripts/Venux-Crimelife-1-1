using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Venux
{
    public static class BanExternal
    {
        public static void BanPlayer(this DbPlayer dbPlayer, string author = "dem automatischem System", string reason = "Automatischer Sicherheitsbann")
        {
            try
            {
                if (dbPlayer == null) return;
                NAPI.Pools.GetAllPlayers().ForEach((Player target) => Notification.SendGlobalNotification(target,
                    "Der Spieler " + dbPlayer.Name + " hat von " + author +
                    " einen permanenten Communityausschluss erhalten.", 8000, "red", Notification.icon.thief));
                dbPlayer.Client.SendNotification("~r~Du wurdest gebannt. Grund: " + reason);

                BanModule.BanIdentifier(dbPlayer.Client.SocialClubName, reason, dbPlayer.Name);
                BanModule.BanIdentifier(dbPlayer.Client.Address, reason, dbPlayer.Name);
                BanModule.BanIdentifier(dbPlayer.Client.Serial, reason, dbPlayer.Name);
                BanModule.BanIdentifier(dbPlayer.Name, reason, dbPlayer.Name);

                dbPlayer.Client.Kick();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION BanPlayer] " + ex.Message);
                Logger.Print("[EXCEPTION BanPlayer] " + ex.StackTrace);
            }
        }

        public static void BanKickPlayer(this Player c, string reason)
        {
            try
            {
                if (c == null) return;
                c.SendNotification("~r~Du wurdest gebannt. Grund: " + reason);
                c.Kick();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION BanKickPlayer] " + ex.Message);
                Logger.Print("[EXCEPTION BanKickPlayer] " + ex.StackTrace);
            }
        }

        public static bool isBanned(this Player c)
        {
            Ban ban = BanModule.bans.Find((Ban ban2) => ban2.Identifier == c.Serial || ban2.Identifier == c.SocialClubName || ban2.Identifier == c.Address);
            if (ban == null) return false;

            return true;
        }

        public static string GetBanReason(this Player c)
        {
            Ban ban = BanModule.bans.Find((Ban ban2) => ban2.Identifier == c.Serial || ban2.Identifier == c.SocialClubName || ban2.Identifier == c.Address);
            if (ban == null) return "";

            return ban.Reason;
        }
    }
}

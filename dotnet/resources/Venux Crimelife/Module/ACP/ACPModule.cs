using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Venux.Module;

namespace Venux
{
    public class ACPModule : Module<ACPModule>
    {
        protected override bool OnLoad()
        {

            return true;
        }

        [RemoteEvent("restartacces")]
        public static void restartacces(Player client, string name)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.Adminrank.Permission > 99)
                {
                    dbPlayer.TriggerEvent("openWindow", "Confirmation", "{\"confirmationObject\":{\"Title\":\"Server Restart\",\"Message\":\"Möchtest du dden Server wirklich neustarten?\",\"Callback\":\"acceptrestart\",\"Arg1\":,\"Arg2\":\"\"}}");
                }
                else
                {
                    dbPlayer.SendNotification("Kein Zugriff!", 3000, "red");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openAdminShop] " + ex.Message);
                Logger.Print("[EXCEPTION openAdminShop] " + ex.StackTrace);
            }
        }

        [RemoteEvent("acceptrestart")]
        public void PhoneJoinfrak(Player c, string frak, object unused)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                Notification.SendGlobalNotification("In 5 Minuten findet ein Server Restart statt! (Grund: Administrativer Neustart)", 8000, "orange", Notification.icon.warn);

                NAPI.Task.Run(() =>
                {
                    Notification.SendGlobalNotification("Der Server wird JETZT neugestartet!", 8000, "orange", Notification.icon.warn);
                }, 300000);
                NAPI.Task.Run(() =>
                {
                    Environment.Exit(0);
                }, 10000);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION acceptInvite] " + ex.Message);
                Logger.Print("[EXCEPTION acceptInvite] " + ex.StackTrace);
            }
        }
    }
}
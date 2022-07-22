using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using Venux.Module;

namespace Venux
{
    public class NativeModule : Module<NativeModule>
    {
        [RemoteEvent("m")]
        public static void nativeMenu(Player client, string id)
        {
            try
            {
                DbPlayer dbPlayer = client.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (string.IsNullOrEmpty(id)) return;

                if (id != "NaN")
                {
                    NativeMenu nativeMenu = (NativeMenu) dbPlayer.GetData("PLAYER_CURRENT_NATIVEMENU");
                    if (nativeMenu != null)
                    {
                        client.Eval("mp.events.callRemote('nM-" + nativeMenu.Title + "', '" +
                                    nativeMenu.Items[Convert.ToInt32(id)].selectionName + "');");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION m] " + ex.Message);
                Logger.Print("[EXCEPTION m] " + ex.StackTrace);
            }
        }
    }
}

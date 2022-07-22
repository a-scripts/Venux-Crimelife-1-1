using System;
using GTANetworkAPI;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public static class Confirmation
    {
        public static void OpenConfirmation(this DbPlayer dbPlayer, ConfirmationObject confirmationObject)
        {
            object confirmationObj = new
            {
                confirmationObject = confirmationObject
            };

            Player c = dbPlayer.Client;

            c.TriggerEvent("openWindow", nameof(Confirmation), NAPI.Util.ToJson(confirmationObj));
        }
    }
}

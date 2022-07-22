using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    class EjectModule : Script
    {

        [RemoteEvent("REQUEST_VEHICLE_EJECT")]
        public void REQUEST_VEHICLE_EJECT(Player client)
        {
            try
            {
                DbPlayer dbPlayer = client.GetPlayer();
                client.TriggerEvent("venux:openseat");
                //dbPlayer.SendNotification("Diese Funktion ist zurzeit deaktiviert!", 3000, "red");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_VEHICLE_DOOR_LOCK] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_VEHICLE_DOOR_LOCK] " + ex.StackTrace);
            }
        }

        [RemoteEvent("seat1")]
        public void seat1(Player client)
        {
            try
            {
                if (client == null) return;
                DbPlayer dbPlayer = client.GetPlayer();
                dbPlayer.SendNotification("TEST BESTANDEN", 3000, "red");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION REQUEST_VEHICLE_DOOR_LOCK] " + ex.Message);
                Logger.Print("[EXCEPTION REQUEST_VEHICLE_DOOR_LOCK] " + ex.StackTrace);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace Venux
{
    public static class Dogfight
    {
        public static void initializeDogfight(this DbPlayer dbPlayer)
        {
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

            dbPlayer.Client.TriggerEvent("initializeDogfight");
        }

        public static void finishDogfight(this DbPlayer dbPlayer)
        {
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;

            dbPlayer.Client.TriggerEvent("finishDogfight");
        }

        public static void updateDogfightScore(this DbPlayer dbPlayer, int kills, int deaths)
        {
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) return;
            float kd = 0;
            if (kills > 0 && deaths > 0) kd = kills / deaths;
            dbPlayer.Client.TriggerEvent("updateDogfightScore", kills, deaths, kd);
        }
    }
}
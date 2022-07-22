using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Venux.Module;

namespace Venux
{
    class AnticheatModule : Module<AnticheatModule>
    {
        [RemoteEvent("checkWeaponHashes")]
        public void checkWeaponHashes(Player c, int weaponHash2)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.HasData("IN_GANGWAR") ||
                    (dbPlayer.HasData("PBZone") && dbPlayer.GetData("PBZone") != null))
                    return;

                WeaponHash weaponHash = (WeaponHash) weaponHash2;
                Adminrank adminranks = dbPlayer.Adminrank;

                if (weaponHash == WeaponHash.Unarmed) return;

                if (dbPlayer.Adminrank.Permission > 93)
                {
                    return;
                }
                if (!dbPlayer.Loadout.Contains(weaponHash))
                {
                    c.RemoveWeapon(weaponHash);
                    c.Vehicle.Delete();
                    dbPlayer.SendNotification("Anticheat hat dich verdächtigt! (Ein Teammitglied wurde kontaktiert!)", 5000, "red");
                    WebhookSender.SendMessage("Neuer Flag",
    "Der Spieler " + dbPlayer.Name + " wurde gerade von dem Anticheat geflaggt. Verdacht: Cheating",
    Webhooks.aclogs, "Flag");

                    Item item = ItemModule.itemRegisterList.Find((Item item2) => item2.Whash == weaponHash);
                    if (item == null)
                    {
                        dbPlayer.BanPlayer();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION checkWeaponHashes] " + ex.Message);
                Logger.Print("[EXCEPTION checkWeaponHashes] " + ex.StackTrace);
            }
        }

        [RemoteEvent("server:CheatDetection")]
        public void CheatDetection(Player c, string reason)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.HasData("PLAYER_ADUTY") && dbPlayer.GetData("PLAYER_ADUTY") == true) return;

                WebhookSender.SendMessage("Neuer Flag",
                    "Der Spieler " + dbPlayer.Name + " wurde gerade von dem Anticheat geflaggt. Verdacht: " + reason,
                    Webhooks.aclogs, "Flag");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheatDetection] " + ex.Message);
                Logger.Print("[EXCEPTION CheatDetection] " + ex.StackTrace);
            }
        }
    }
}

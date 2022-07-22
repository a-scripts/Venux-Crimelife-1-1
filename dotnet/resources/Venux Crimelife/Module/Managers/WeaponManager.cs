using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    class WeaponManager : Script
    {
        public static void removeWeapon(Player client, WeaponHash weaponHash)
        {
            DbPlayer dbPlayer = client.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            List<WeaponHash> playerWeapons = NAPI.Util.FromJson<List<WeaponHash>>(dbPlayer.GetAttributeString("Loadout"));

            if (playerWeapons.Contains(weaponHash))
                playerWeapons.Remove(weaponHash);

            dbPlayer.Loadout = playerWeapons;
            dbPlayer.RefreshData(dbPlayer);

            client.RemoveWeapon(weaponHash);

            dbPlayer.SetAttribute("Loadout", NAPI.Util.ToJson(new List<WeaponHash>()));
        }

        public static void loadWeapons(Player c)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            List<WeaponHash> playerWeapons = NAPI.Util.FromJson<List<WeaponHash>>(dbPlayer.GetAttributeString("Loadout"));

            dbPlayer.Loadout = playerWeapons;
            dbPlayer.RefreshData(dbPlayer);

            c.RemoveAllWeapons();

            foreach (WeaponHash weapon in playerWeapons)
            {
                c.GiveWeapon(weapon, 9999);
            }
            NAPI.Player.SetPlayerCurrentWeapon(c, WeaponHash.Unarmed);
        }

        public static void addWeapon(Player client, WeaponHash weaponHash)
        {
            DbPlayer dbPlayer = client.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            client.GiveWeapon(weaponHash, 9999);

            List<WeaponHash> playerWeapons = NAPI.Util.FromJson<List<WeaponHash>>(dbPlayer.GetAttributeString("Loadout"));
            playerWeapons.Add(weaponHash);

            dbPlayer.Loadout = playerWeapons;
            dbPlayer.RefreshData(dbPlayer);

            dbPlayer.SetAttribute("Loadout", NAPI.Util.ToJson(playerWeapons));
        }

        public static void removeAllWeapons(Player client)
        {
            DbPlayer dbPlayer = client.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            client.RemoveAllWeapons();

            dbPlayer.Loadout.Clear();
            dbPlayer.RefreshData(dbPlayer);

            dbPlayer.SetAttribute("Loadout", NAPI.Util.ToJson(new List<WeaponHash>()));
        }
    }
}

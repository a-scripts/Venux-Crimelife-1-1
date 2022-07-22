﻿/*using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTANetworkAPI;
using Venux.Module;

namespace Venux
{
    class AdminApp : Module<AdminApp>
    {
        [RemoteEvent("requestAdminMembers")]
        public static void requestAdminMembers(Client c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<AdminMember> TeamMemberList = new List<AdminMember>();

                int ManagePermission = 0;

                if (dbPlayer.Factionrank > 9 && dbPlayer.Factionrank != 12)
                {
                    ManagePermission = 1;
                }
                else if (dbPlayer.Factionrank == 12)
                {
                    ManagePermission = 2;
                }

                foreach (DbPlayer fraktionPlayer in dbPlayer.Faction.GetFactionPlayers())
                {
                    int manage = 0;
                    bool medic = true;

                    if (fraktionPlayer.Factionrank > 10 && fraktionPlayer.Factionrank != 12)
                    {
                        manage = 1;
                    }
                    else if (fraktionPlayer.Factionrank == 12)
                    {
                        manage = 2;
                    }
                    else if (fraktionPlayer.Medic == true)
                    {
                        medic = true;
                    }

                    TeamMemberList.Add(new AdminMember
                    {
                        Id = fraktionPlayer.Id,
                        Name = fraktionPlayer.Name,
                        Rank = fraktionPlayer.Factionrank,
                        Manage = (fraktionPlayer.Factionrank > 9 ? 2 : 0),
                        Medic = (fraktionPlayer.Medic == true),
                        Number = fraktionPlayer.Id
                    });
                }

                TeamMemberList = TeamMemberList.OrderBy(obj => obj.Rank).ToList();
                TeamMemberList.Reverse();

                object JSONobject = new
                {
                    TeamMemberList = TeamMemberList,
                    ManagePermission = ManagePermission
                };

                dbPlayer.TriggerEvent("componentServerEvent", "AdminListApp", "responseAdminMembers",
                    NAPI.Util.ToJson(JSONobject));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION requestTeamMembers] " + ex.Message);
                Logger.Print("[EXCEPTION requestTeamMembers] " + ex.StackTrace);
            }
        }

        [RemoteEvent("leaveFrak")]
        public void LeaveFrak(Client c)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {

                if (dbPlayer.Faction.Id == 0)
                    return;

                dbPlayer.SetAttribute("Fraktion", 0);
                dbPlayer.SetAttribute("Fraktionrank", 0);

                dbPlayer.Faction = FactionModule.getFactionById(0);
                dbPlayer.Factionrank = 0;
                dbPlayer.RefreshData(dbPlayer);
                c.TriggerEvent("updateTeamId", 0);
                c.TriggerEvent("updateTeamRank", 0);
                c.TriggerEvent("updateJob", "Zivilist");

                dbPlayer.SendNotification("Du hast die Fraktion verlassen.", 3000, "orange", "fraktionssystem");
                c.TriggerEvent("hatNudeln", false);

                if (dbPlayer.Faction.Name == "Zivilist")
                    return;
                foreach (DbPlayer target in dbPlayer.Faction.GetFactionPlayers())
                {
                    target.SendNotification("Der Spieler " + c.Name + " hat die Fraktion verlassen.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                }

            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION leaveFrak] " + ex.Message);
                Logger.Print("[EXCEPTION leaveFrak] " + ex.StackTrace);
            }
        }

        [RemoteEvent("clearFraktionV")]
        public void clearFraktion(Client c)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {

                if (dbPlayer.Faction.Id == 0)
                    return;

                if (dbPlayer.Factionrank == 12)

                foreach (Client target in NAPI.Pools.GetAllPlayers())
                {
                    DbPlayer dbPlayer1 = target.GetPlayer();
                    if (dbPlayer1.Factionrank == 12) return;
                    if (dbPlayer1.Faction.Id < 0);
                    {
                          dbPlayer1.SetAttribute("Fraktion", 0);
                          dbPlayer1.SetAttribute("Fraktionrank", 0);
                          dbPlayer1.Faction = FactionModule.getFactionById(0);
                          dbPlayer1.Factionrank = 0;
                          dbPlayer1.RefreshData(dbPlayer);
                          target.TriggerEvent("updateTeamId", 0);
                          target.TriggerEvent("updateTeamRank", 0);
                          target.TriggerEvent("updateJob", "Zivilist");
                        }
                }
                dbPlayer.SendNotification("Deine Fraktion wurde erfolgreich gecleart!", 3000, "orange", "fraktionssystem");
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION leaveFrak] " + ex.Message);
                Logger.Print("[EXCEPTION leaveFrak] " + ex.StackTrace);
            }
        }

        [RemoteEvent("editTeamMember")]
        public void editTeamMember(Client c, string name, int newrank)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                if (dbPlayer.Faction.Id == 0)
                    return;

                if (dbPlayer.Factionrank > 9)
                {

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    {
                        dbPlayer.SendNotification("Spieler nicht online!", 3000, "red");
                        return;
                    }

                    if (dbPlayer2.Faction.Id == dbPlayer.Faction.Id)
                    {
                        if (dbPlayer2.Factionrank < dbPlayer.Factionrank)
                        {
                            dbPlayer.SendNotification("Du hast den Spieler " + name + " auf Rang " + newrank + " gestuft.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);

                            dbPlayer2.SetAttribute("Fraktionrank", newrank);
                            dbPlayer2.Factionrank = newrank;
                            dbPlayer2.RefreshData(dbPlayer2);

                            dbPlayer2.SendNotification("Dein Rang wurde auf " + newrank + " gestuft.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                        }
                        else
                        {
                            dbPlayer.SendNotification("Du hast keine Berechtigung, um die Rechte für diesen Spieler zu verändern.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                        }
                    }
                    else
                    {
                        dbPlayer.SendNotification("Dieser Spieler ist nicht in deiner Fraktion.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Du hast dazu keine Berechtigung.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION editTeamMember] " + ex.Message);
                Logger.Print("[EXCEPTION editTeamMember] " + ex.StackTrace);
            }
        }

        [RemoteEvent("parkFraktionVehicles")]
        public void parkCars(Client c)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                if (dbPlayer.Faction == null || dbPlayer.Faction.Id == 0)
                    return;

                 dbPlayer.Faction.GetFactionPlayers().ForEach((DbPlayer target) => target.SendNotification("Alle Fraktionsfahrzeuge wurden von " + c.Name + " eingeparkt.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name));
                //dbPlayer.SendNotification("Diese Funktion ist aktuell deaktiviert!", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);

                foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                {
                    DbVehicle dbVehicle = vehicle.GetVehicle();
                    if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                        return;
                    if (dbVehicle.Plate == dbPlayer.Faction.Short.ToUpper())
                        vehicle.Delete();
                                    }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION parkCars] " + ex.Message);
                Logger.Print("[EXCEPTION parkCars] " + ex.StackTrace);
            }
        }

        [RemoteEvent("kickMember")]
        public void PhoneUninvite(Client c, string name)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {

                if (dbPlayer.Faction == null || dbPlayer.Faction.Id == 0)
                    return;

                if (dbPlayer.Factionrank > 9)
                {
                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    {
                        dbPlayer.SendNotification("Spieler nicht online!", 3000, "red");
                        return;
                    }

                    if (dbPlayer2.Faction.Id == dbPlayer.Faction.Id)
                    {
                        if (dbPlayer2.Factionrank < dbPlayer.Factionrank)
                        {
                            dbPlayer.SendNotification("Du hast den Spieler " + name + " uninvited.", 3000,
                                dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);

                            dbPlayer2.SetAttribute("Fraktion", 0);
                            dbPlayer2.SetAttribute("Fraktionrank", 0);

                            dbPlayer2.TriggerEvent("updateTeamId", 0);
                            dbPlayer2.TriggerEvent("updateTeamRank", 0);
                            dbPlayer2.TriggerEvent("updateJob", "Zivilist");

                            dbPlayer2.Faction = FactionModule.getFactionById(0);
                            dbPlayer2.Factionrank = 0;
                            dbPlayer2.RefreshData(dbPlayer2);
                            dbPlayer2.SendNotification("Du wurdest aus der Fraktion " + dbPlayer.Faction.Name + " gekickt.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                        }
                        else
                        {
                            dbPlayer.SendNotification("Du hast keine Berechtigung, um diesen Spieler zu uninviten.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                        }
                    }
                    else
                    {
                        dbPlayer.SendNotification("Dieser Spieler ist nicht in deiner Fraktion.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Du hast dazu keine Berechtigung.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION kickMember] " + ex.Message);
                Logger.Print("[EXCEPTION kickMember] " + ex.StackTrace);
            }
        }

        [RemoteEvent("givemedicvenux")]
        public void Phonemedic(Client c, string name)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            List<DbPlayer> list = dbPlayer.Faction.GetFactionPlayers().FindAll((DbPlayer player) => player.Medic);
            Client client2 = dbPlayer.Client;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                if (dbPlayer.Faction.Id != 0 && dbPlayer.Factionrank >= 11)
                {
                    DbPlayer player2 = PlayerHandler.GetPlayer(name);
                    if (player2 == null || !player2.IsValid(ignorelogin: true))
                    {
                        dbPlayer.SendNotification("Spieler nicht gefunden.");
                    }
                    else if (dbPlayer.Faction.Id != player2.Faction.Id)
                    {
                        dbPlayer.SendNotification("Ihr seit nicht in der gleichen Fraktion!");
                    }
                    else if (player2.Medic)
                    {
                        player2.Medic = false;
                        player2.RefreshData(player2);
                        player2.SetAttribute("Medic", 0);
                        player2.SendNotification("Du bist nun kein Frak-Medic mehr!", 5000, "red", dbPlayer.Faction.Name);
                        dbPlayer.SendNotification(""+player2.Name+" ist nun kein Frak-Medic mehr!", 5000, "red", dbPlayer.Faction.Name);
                    }
                    else
                    {
                        if (list.Count >= 5)
                        {
                            stringBuilder = new StringBuilder();
                            list.ForEach(delegate (DbPlayer player)
                            {
                                stringBuilder.Append(player.Name + ", ");
                            });
                            dbPlayer.SendNotification("Es sind bereits 5 Medics in deiner Fraktion. " + stringBuilder.ToString());
                        }
                        else
                        {
                            Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(player2.Faction.Id));
                            player2.Medic = true;
                            player2.RefreshData(player2);
                            player2.SetAttribute("Medic", 1);
                            player2.SendNotification("Du bist nun Frak-Medic!", 5000, "red", dbPlayer.Faction.Name);
                            dbPlayer.SendNotification(""+player2.Name+" ist nun Frak-Medic!", 5000, fraktion.GetRGBStr(), dbPlayer.Faction.Name);
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                Logger.Print("[EXCEPTION setmedic] " + ex2.Message);
                Logger.Print("[EXCEPTION setmedic] " + ex2.StackTrace);
            }
        }

        [RemoteEvent("addPlayer")]
        public void PhoneInvite(Client c, string name)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
              return;
            try
            {
                if (dbPlayer.Faction == null || dbPlayer.Faction.Id == 0)
                    return;

                if (dbPlayer.Factionrank > 9)
                {
                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                    if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    {
                        dbPlayer.SendNotification("Spieler nicht online!", 3000, "red");
                        return;
                    }



                    if (dbPlayer2.Faction.Id == dbPlayer.Faction.Id)
                    {
                        dbPlayer.SendNotification("Der Spieler ist bereits in deiner Fraktion.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                    }
                    else
                    {
                        if (dbPlayer2.Faction.Id == 0)
                        {
                            dbPlayer2.TriggerEvent("openWindow", "Confirmation", "{\"confirmationObject\":{\"Title\":\"" + dbPlayer.Faction.Name + "\",\"Message\":\"Möchtest du die Einladung von " + c.Name + " annehmen?\",\"Callback\":\"acceptInvite\",\"Arg1\":" + dbPlayer.Faction.Id + ",\"Arg2\":\"\"}}");
                            dbPlayer.SendNotification("Du hast " + name + " eine Einladung gesendet.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                        }
                        else
                        {
                            dbPlayer.SendNotification("Dieser Spieler ist bereits in einer Fraktion.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                        }
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Du hast dazu keine Berechtigung.", 3000, dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION addPlayer] " + ex.Message);
                Logger.Print("[EXCEPTION addPlayer] " + ex.StackTrace);
            }
        }


        [RemoteEvent("acceptInvite")]
        public void PhoneJoinfrak(Client c, string frak, object unused)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(frak));
                dbPlayer.Faction = fraktion;
                dbPlayer.Factionrank = 0;
                dbPlayer.RefreshData(dbPlayer);

                dbPlayer.SetAttribute("Fraktion", fraktion.Id);
                dbPlayer.SetAttribute("Fraktionrank", 0);

                c.TriggerEvent("updateTeamId", fraktion.Id);
                c.TriggerEvent("updateTeamRank", 0);
                c.TriggerEvent("updateJob", fraktion.Name);

                foreach (DbPlayer target in fraktion.GetFactionPlayers())
                {
                    target.SendNotification("" + c.Name + " ist jetzt ein Mitglied", 3000, fraktion.GetRGBStr(), fraktion.Name);
                }

                dbPlayer.SendNotification("Du bist der Fraktion " + fraktion.Name + " beigetreten.", 3000, fraktion.GetRGBStr(), fraktion.Name);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION acceptInvite] " + ex.Message);
                Logger.Print("[EXCEPTION acceptInvite] " + ex.StackTrace);
            }
        }


    }
}*/

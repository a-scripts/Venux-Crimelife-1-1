using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Venux.Handlers;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Venux
{   
    public class Main : Script
    {
        public static int timeToRestart;

        public void InitGameMode()
        {

            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetCommandErrorMessage(" ");
            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetAutoSpawnOnConnect(false);

            Modules.Instance.LoadAll();

            Logger.Print("");
            Logger.Print("  =========================================================    ");
            Logger.Print("");
            Logger.Print("");
            Logger.Print("  K S C R I P T S  C R I M E L I F E  1 . 1");
            Logger.Print("");
            Logger.Print("  S T A R T E D  G A M E S E R V E R");
            Logger.Print("");
            Logger.Print("");
            Logger.Print("  =========================================================    ");
            Logger.Print("");

            MySqlHandler.ExecuteSync(new MySqlQuery("UPDATE vehicles SET Parked = 1"));
        }

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStartHandler()
        {
            InitGameMode();
            timeToRestart = 36000;
            Integration.DiscordIntegration.SetUpBotInstance("OTIyMTY0MjkyODIyODkyNjA1.Yb9ebA.snP4TFQN1XBv4d7KKWfQERiGxcA", ""+NAPI.Pools.GetAllPlayers().Count+" Spieler auf dem Server", Discord.ActivityType.Playing, Discord.UserStatus.DoNotDisturb);
            SyncThread.Init();
            SyncThread.Instance.Start();
        }

        public static void OnHourHandler()
        {
            try
            {
                foreach (DbPlayer dbPlayer in PlayerHandler.GetPlayers())
                {
                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                        continue;

                    dbPlayer.SetAttribute("Level", (int)dbPlayer.GetAttributeInt("Level") + 1);
                    dbPlayer.Level = dbPlayer.Level + 1;
                    dbPlayer.RefreshData(dbPlayer);

                    House house = HouseModule.houses.Find((House house2) => house2.TenantsIds.Contains(dbPlayer.Id));

                    if (house != null)
                    {
                        int price = 0;

                        if (house.TenantPrices.ContainsKey(dbPlayer.Id))
                            price = house.TenantPrices[dbPlayer.Id];

                        dbPlayer.SendNotification("Dir wurde dein Mietpreis abgezogen! -" + price.ToDots() + "$");
                        dbPlayer.removeMoney(price);
                    }

                    dbPlayer.addMoney(150000);
                    dbPlayer.SendNotification("Du hast deinen PayDay erhalten! (+150.000$)", 3000, "green");
                    Adminrank adminranks = dbPlayer.Adminrank;

                    if (adminranks.Permission >= 91)
                    {
                        dbPlayer.SendNotification("Du bist ein Teammitglied mit dem Rang `RANG` und hast dadurch dein Teamgehalt dazu bekommen! (+250.000$)", 3000, "red");
                        dbPlayer.addMoney(250000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION OnHourSpent] " + ex.Message);
                Logger.Print("[EXCEPTION OnHourSpent] " + ex.StackTrace);
            }
        }


        public static void OnThreeHandler()
        {
            try
            {
                foreach (DbPlayer dbPlayer in PlayerHandler.GetPlayers())
                {
                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                        continue;

                    Notification.SendGlobalNotification("In 5 Minuten findet ein Server Restart statt! (Grund: Automatischer Neustart)", 8000, "orange", Notification.icon.warn);

                    NAPI.Task.Run(() =>
                    {
                        Notification.SendGlobalNotification("Der Server wird JETZT neugestartet!", 8000, "orange", Notification.icon.warn);
                    }, 300000);
                    NAPI.Task.Run(() =>
                    {
                        Environment.Exit(0);
                    }, 10000);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION OnThreeSpent] " + ex.Message);
                Logger.Print("[EXCEPTION OnThreeSpent] " + ex.StackTrace);
            }
        }

        public static void OnMinHandler()
        {
            try
            {
                MySqlConnection con = new MySqlConnection(Configuration.connectionString);
                con.ClearAllPoolsAsync();
                con.Dispose();
                /*foreach (Client client in NAPI.Pools.GetAllPlayers())
                {
                    DbPlayer dbPlayer = client.GetPlayer();
                    if ((dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null) && client.Dimension == 0)
                    {
                        client.SendNotification("Ungültiger Account!");
                        client.Kick();
                    }
                }
                */
                
                foreach (DbPlayer dbPlayer in PlayerHandler.GetPlayers())
                {
                    if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.Client.IsNull)
                        continue;

                    if (dbPlayer.DeathData.IsDead)
                    {
                        if (dbPlayer.Client == null) return;
                        DeathData deathData = dbPlayer.DeathData;
                        DateTime dateTime = deathData.DeathTime;
                        dbPlayer.disableAllPlayerActions(true);
                        dbPlayer.SetInvincible(true);
                        ComponentManager.Get<DeathWindow>().Show()(dbPlayer);
                        dbPlayer.SendNotification("Du liegst nun im Koma keiner kann dich retten!", 3000);
                        dbPlayer.StopAnimation();
                        dbPlayer.PlayAnimation(33, "combat@damage@rb_writhe", "rb_writhe_loop", 8f);

                        if (DateTime.Now.Subtract(dateTime).TotalMinutes >= 2)
                        {
                            dbPlayer.DeathData = new DeathData
                            {
                                IsDead = false,
                                DeathTime = new DateTime(0)
                            };

                            if (dbPlayer.Faction.Id == 0)
                                dbPlayer.SpawnPlayer(new Vector3(298.08, -584.53, 43.26));
                            else
                                dbPlayer.SpawnPlayer(dbPlayer.Faction.Spawn);

                            dbPlayer.disableAllPlayerActions(false);
                            dbPlayer.StopAnimation();
                            dbPlayer.StopScreenEffect("DeathFailOut");
                            ComponentManager.Get<DeathWindow>().Close(dbPlayer.Client);
                            dbPlayer.SetAttribute("Death", 0);
                            dbPlayer.SetInvincible(false);
                            dbPlayer.SetHealth(200);
                            dbPlayer.SetArmor(0);

                            if (dbPlayer.Client.Dimension != FactionModule.GangwarDimension)
                            {
                                dbPlayer.GetInventoryItems().ForEach((ItemModel itemModel) => dbPlayer.UpdateInventoryItems(itemModel.Name, itemModel.Amount, true));
                                dbPlayer.RemoveAllWeapons(true);
                            }                           
                        }
                    }

                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Id = @userId LIMIT 1");
                    mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    MySqlDataReader reader = mySqlReaderCon.Reader;
                    try
                    {
                        /*if (!reader.HasRows)
                        {
                            dbPlayer.Client.SendNotification("Ungültiger Account!");
                            dbPlayer.Client.Kick();
                            continue;
                        }
                        else*/
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.GetInt32("Fraktion") != dbPlayer.Faction.Id)
                                {
                                    Faction oldfraktion = dbPlayer.Faction;
                                    Faction newfraktion = FactionModule.getFactionById(reader.GetInt32("Fraktion"));

                                    dbPlayer.Faction = newfraktion;
                                    dbPlayer.RefreshData(dbPlayer);
                                }

                                if (reader.GetInt32("Fraktionrank") != dbPlayer.Factionrank)
                                {
                                    dbPlayer.Factionrank = reader.GetInt32("Fraktionrank");
                                    dbPlayer.RefreshData(dbPlayer);
                                }
                                if (reader.GetInt32("Business") != dbPlayer.Business.Id)
                                {
                                    Business businessById = BusinessModule.getBusinessById(reader.GetInt32("Business"));
                                    dbPlayer.Business = businessById;
                                    dbPlayer.RefreshData(dbPlayer);
                                }
                                if (reader.GetInt32("Businessrank") != dbPlayer.Businessrank)
                                {
                                    dbPlayer.Businessrank = reader.GetInt32("Businessrank");
                                    dbPlayer.RefreshData(dbPlayer);
                                }
                                if (reader.GetInt32("Adminrank") != dbPlayer.Adminrank.Permission)
                                {
                                    dbPlayer.Adminrank = AdminrankModule.getAdminrank(reader.GetInt32("adminrank"));
                                    dbPlayer.RefreshData(dbPlayer);
                                }

                                if (reader.GetInt32("Money") != dbPlayer.Money)
                                {
                                    dbPlayer.Money = reader.GetInt32("Money");
                                    dbPlayer.RefreshData(dbPlayer);
                                }
                            }
                        }
                    }
                    finally
                    {
                        reader.Dispose();
                        mySqlReaderCon.Connection.Dispose();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION OnMinuteSpent] " + ex.Message);
                Logger.Print("[EXCEPTION OnMinuteSpent] " + ex.StackTrace);
            }
        }
    }
}

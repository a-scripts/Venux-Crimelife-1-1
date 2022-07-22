using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Venux.Module;
using Venux.Module.Player;

namespace Venux
{
    class CommandModule : Module<CommandModule>
    {

        public static List<Faction> factionList = new List<Faction>();
        public static List<Command> commandList = new List<Command>();
        public static List<ClothingModel> clothingList = new List<ClothingModel>();

        protected override bool OnLoad()
        {

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Spieler: " + NAPI.Pools.GetAllPlayers().Count + " Insgesamt, Eingeloggte Spieler auf dem Gameserver: " + PlayerHandler.GetPlayers().Count);
            }, "onlist", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Zurzeit sind: "+ PlayerHandler.GetAdminPlayers().Count +" Teammitglieder auf dem Server!");
            }, "adminlist", 91, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Zurzeit ist das Feature deaktiviert. (Grund: Bearbeitung)");
                dbPlayer.TriggerEvent("venux:openseat");
            }, "news", 91, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    Item item = ItemModule.itemRegisterList.Find((Item x) => x.Name == args[1]);
                    if (item == null) return;

                    dbPlayer.UpdateInventoryItems(item.Name, Convert.ToInt32(args[2]), false);
                    dbPlayer.SendNotification("Du hast dir das Item " + item.Name + " gegeben.", 3000, "green", "Inventar");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION ADDITEM] " + ex.Message);
                    Logger.Print("[EXCEPTION ADDITEM] " + ex.StackTrace);
                }
            }, "additem", 96, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer == null || !dbPlayer.IsValid())
                {
                    return;
                }
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                try
                {

                if (dbPlayer2.Id == dbPlayer.Id)
                {
                    dbPlayer.SendNotification("Sich selber abhören macht keinen Sinn, du Fuchs.", 3000, "red", "ADMIN-COMMAND");
                    return;
                }

                string voiceHashPush = dbPlayer2.VoiceHash;
                dbPlayer.SetData("adminHearing", voiceHashPush);
                dbPlayer.TriggerEvent("setAdminVoice", voiceHashPush);
                dbPlayer.SendNotification($"Abhören begonnen! Spieler: {dbPlayer2.Name}", 3000, "red", "ADMIN-COMMAND");
            }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION ABHÖREN] " + ex.Message);
                    Logger.Print("[EXCEPTION ABHÖREN] " + ex.StackTrace);
                }
            }, "avoice", 98, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                try
                {

                    if (dbPlayer == null || !dbPlayer.IsValid())
                    {
                        return;
                    }

                    if (dbPlayer.HasData("adminHearing"))
                    {
                        dbPlayer.TriggerEvent("clearAdminVoice");
                        dbPlayer.ResetData("adminHearing");
                        dbPlayer.SendNotification("Abhören beendet!", 3000, "red", "ADMIN-COMMAND");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION ABHÖREN] " + ex.Message);
                    Logger.Print("[EXCEPTION ABHÖREN] " + ex.StackTrace);
                }
            }, "endavoice", 98, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Du hast nun einen Spawnschutz von 30 Sekunden! (" + dbPlayer.Name + ")", 3000);
                dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", true);
                //dbPlayer.SetData("PLAYER_ADUTY", true);
                NAPI.Task.Run(() =>
                {
                    dbPlayer.SendNotification("Du hast nun keinen Spawnschutz mehr!", 3000);
                    dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", false);
                    dbPlayer.ResetData("PLAYER_ADUTY");
                }, 30000);
            }, "testc", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                try
                {
                    Player client = dbPlayer.Client;

                    if (!dbPlayer.HasData("PLAYER_ADUTY"))
                    {
                        dbPlayer.SetData("PLAYER_ADUTY", false);
                    }

                    dbPlayer.ACWait();

                    WebhookSender.SendMessage("Spieler wechselt Aduty", "Der Spieler " + dbPlayer.Name + " hat den Adminmodus " + (dbPlayer.GetData("PLAYER_ADUTY") ? "verlassen" : "betreten") + ".", Webhooks.adminlogs, "Admin");

                    client.TriggerEvent("setPlayerAduty", !dbPlayer.GetData("PLAYER_ADUTY"));
                    client.TriggerEvent("updateAduty", !dbPlayer.GetData("PLAYER_ADUTY"));
                    dbPlayer.SetData("PLAYER_ADUTY", !dbPlayer.GetData("PLAYER_ADUTY"));
                    dbPlayer.SpawnPlayer(new Vector3(client.Position.X, client.Position.Y, client.Position.Z + 0.52f));
                    if (dbPlayer.GetData("PLAYER_ADUTY"))
                    {
                        dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", true);
                        Adminrank adminrank = dbPlayer.Adminrank;
                        int num = (int)adminrank.ClothingId;
                        dbPlayer.SetClothes(3, 9, 0);
                        PlayerClothes.setAdmin(dbPlayer, num);
                        dbPlayer.SetClothes(5, 0, 0);
                        return;
                    }
                    else
                    {
                        dbPlayer.Client.SetSharedData("PLAYER_INVINCIBLE", false);
                    }
                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @user LIMIT 1");
                    mySqlQuery.AddParameter("@user", client.Name);

                    MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                    try
                    {
                        MySqlDataReader reader = mySqlReaderCon.Reader;
                        try
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PlayerClothes playerClothes = NAPI.Util.FromJson<PlayerClothes>(reader.GetString("Clothes"));

                                    client.SetAccessories(0, playerClothes.Hut.drawable, playerClothes.Hut.texture);
                                    //client.SetAccessories(1, playerClothes.Brille.drawable, playerClothes.Brille.texture);
                                    dbPlayer.SetClothes(11, playerClothes.Oberteil.drawable, playerClothes.Oberteil.texture);
                                    dbPlayer.SetClothes(1, playerClothes.Maske.drawable, playerClothes.Maske.texture);
                                    dbPlayer.SetClothes(8, playerClothes.Unterteil.drawable, playerClothes.Unterteil.texture);
                                    // dbPlayer.SetClothes(7, playerClothes.Kette.drawable, playerClothes.Kette.texture);
                                    dbPlayer.SetClothes(3, playerClothes.Koerper.drawable, playerClothes.Koerper.texture);
                                    dbPlayer.SetClothes(4, playerClothes.Hose.drawable, playerClothes.Hose.texture);
                                    dbPlayer.SetClothes(6, playerClothes.Schuhe.drawable, playerClothes.Schuhe.texture);
                                }
                            }
                        }
                        finally
                        {
                            reader.Dispose();
                        }
                    }
                    finally
                    {
                        mySqlReaderCon.Connection.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION ADUTY] " + ex.Message);
                    Logger.Print("[EXCEPTION ADUTY] " + ex.StackTrace);
                }
            }, "aduty", 91, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                try
                {
                    Item item = ItemModule.itemRegisterList.Find((Item x) => x.Name == "CaillouCard");
                    if (item == null) return;

                    dbPlayer2.UpdateInventoryItems(item.Name, Convert.ToInt32(1), false);
                    dbPlayer.SendNotification("Der Spieler " + name + " kann nun in das Casino", 3000, "lightblue", "CASINO");
                    dbPlayer2.SendNotification("Du Kannst in das Casino!", 3000, "lightblue", "CASINO");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CASINO] " + ex.Message);
                    Logger.Print("[EXCEPTION CASINO] " + ex.StackTrace);
                }
            }, "casino", 97, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 == null)
                    return;

                Adminrank adminrank = dbPlayer.Adminrank;
                Adminrank adminranks = dbPlayer2.Adminrank;

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                    if (adminrank.Permission <= adminranks.Permission)
                    {
                        dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                        return;
                    }
                    else
                    {
                        Player client = dbPlayer2.Client;
                        client.TriggerEvent("openWindow", new object[2]
{
                                   "Kick",
                                    "{\"name\":\""+ dbPlayer2.Name +"\",\"grund\":\"" + String.Join(" ", args).Replace("kick " + name, "") +"\"}"
});
                        dbPlayer2.Client.Kick();
                        dbPlayer.SendNotification("Spieler gekickt!", 3000, "red");
                        Notification.SendGlobalNotification("" + dbPlayer2.Name + " wurde von " + dbPlayer.Name + " gekickt. Grund: " + String.Join(" ", args).Replace("kick " + name, ""), 8000, "red", Notification.icon.warn);
                       // String.Join(" ", args).Replace("announce ", "")
                    }
            }, "kick", 1, 1));

            
            commandList.Add(new Command((dbPlayer, args) =>
            {
                string anim1 = args[1];
                string anim2 = args[2];

                Player client = dbPlayer.Client;
                NAPI.Player.PlayPlayerAnimation(client, 49, ""+anim1+"", ""+anim2+"", 8f);

            }, "testanim", 99, 1));
           

            commandList.Add(new Command((dbPlayer, args) =>
            {

                if (dbPlayer.DeathData.IsDead) return;

                Player client = dbPlayer.Client;

                if (dbPlayer.Faction.Id != 0 && dbPlayer.Factionrank > 9)
                {
                    List<BuyCar> Cars = new List<BuyCar>();

                    List<string> vehicles = new List<string>();

                    FactionModule.VehicleList[dbPlayer.Faction.Id].ForEach((GarageVehicle garageVehicle) =>
                    {
                        vehicles.Add(garageVehicle.Name);
                    });

                    if (!vehicles.Contains("neon"))
                    {
                        Cars.Add(new BuyCar("neon", 28000000));
                    }
                    if (!vehicles.Contains("jugular"))
                    {
                        Cars.Add(new BuyCar("jugular", 23000000));
                    }
                    if (!vehicles.Contains("drafter"))
                    {
                        Cars.Add(new BuyCar("drafter", 12000000));
                    }
                    if (!vehicles.Contains("revolter"))
                    {
                        Cars.Add(new BuyCar("revolter", 18000000));
                    }
                    if (!vehicles.Contains("bf400"))
                    {
                        Cars.Add(new BuyCar("bf400", 1));
                    }
                    if (!vehicles.Contains("schafterg"))
                    {
                        Cars.Add(new BuyCar("schafterg", 1));
                    }
                    if (!vehicles.Contains("bati"))
                    {
                        Cars.Add(new BuyCar("bati", 4000000));
                    }
                    if (!vehicles.Contains("caddy"))
                    {
                        Cars.Add(new BuyCar("caddy", 20000000));
                    }
                    if (!vehicles.Contains("supervolito2"))
                    {
                        Cars.Add(new BuyCar("supervolito2", 60000000));
                    }
                    if (!vehicles.Contains("benson"))
                    {
                        Cars.Add(new BuyCar("benson", 45000000));
                    }

                    List<NativeItem> Items = new List<NativeItem>();

                    foreach (BuyCar buyCar in Cars)
                    {
                        Items.Add(new NativeItem(buyCar.Vehicle_Name + " - " + buyCar.Price.ToDots() + "$", buyCar.Vehicle_Name.ToLower() + "-" + buyCar.Price));
                    }

                    NativeMenu nativeMenu = new NativeMenu("Leadershop", dbPlayer.Faction.Short, Items);
                    dbPlayer.ShowNativeMenu(nativeMenu);
                }
            }, "leadershop", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    dbPlayer.SendNotification("X: " + Math.Round(client.Position.X, 2) + " Y: " + Math.Round(client.Position.Y, 2) + " Z: " + Math.Round(client.Position.Z, 2) + " Heading: " + Math.Round(client.Heading, 2), 60000, "green", "");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "pos", 1, 0));

            commandList.Add(new Command((dbPlayer, args) => FactionBank.OpenFactionBank(dbPlayer), "frakbank", 0, 0));

            commandList.Add(new Command((dbPlayer, args) => BusinessBank.OpenBusinessBank(dbPlayer), "businessbank", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                Notification.SendGlobalNotification("Es wurden Administrativ alle Fahrzeuge eingeparkt.", 5000, "white", Notification.icon.bullhorn);
                NAPI.Pools.GetAllVehicles().ForEach((Vehicle veh) => veh.Delete());
                MySqlHandler.ExecuteSync(new MySqlQuery("UPDATE vehicles SET Parked = 1"));
            }, "parkall", 97, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                Notification.SendGlobalNotification(String.Join(" ", args).Replace("announce ", ""), 10000, "white", Notification.icon.bullhorn);
            }, "announce", 96, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                Notification.SendGlobalNotification("Das Casino hat nun Geöffnet, Tickets könnt ihr vorort kaufen!", 10000, "lightblue", Notification.icon.diamond);
            }, "casinoa", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                Notification.SendGlobalNotification("Das Casino schließt nun, vielen dank für ihr Besuch!", 10000, "lightblue", Notification.icon.diamond);
            }, "casinoc", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string text = args[1];
                Player client = dbPlayer.Client;
                Notification.SendGlobalNotification(String.Join("", "Die Fraktion " + text + " hat eine Offene Bewerbungsphase! (15 Minuten Safezone)").Replace("bwp ", ""), 10000, "white", Notification.icon.bullhorn);
            }, "bwp", 94, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                if (!client.IsInVehicle) return;
                dbPlayer.SendNotification("Fahrzeug gelöscht!", 3000, "red", "ADMIN");
                client.Vehicle.Delete();
            }, "dv2", 1, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.HasData("disablenc"))
                {
                    dbPlayer.SendNotification("TeamChat und Support wurde AKTIVIERT!", 3000, "green");
                    dbPlayer.ResetData("disablenc");
                }
                else
                {
                    dbPlayer.SendNotification("TeamChat und Support wurde DEAKTIVIERT!", 3000, "red");
                    dbPlayer.SetData("disablenc", true);
                }
            }, "toggle", 94, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                DbVehicle dbVehicle = client.Vehicle.GetVehicle();
                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                    return;
                if (!client.IsInVehicle) return;
                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET Parked = 1 WHERE Id = @id");
                mySqlQuery.AddParameter("@id", dbVehicle.Id);
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Fahrzeug eingeparkt!", 3000, "red", "ADMIN");
                client.Vehicle.Delete();
            }, "dv", 1, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                float dist = 0;
                bool dist2 = float.TryParse(args[1], out dist);
                
                if (!dist2) return;

                foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                {
                    if (vehicle.Position.DistanceTo(dbPlayer.GetPosition()) <= dist)
                    {
                        vehicle.Delete();
                    }
                }
                dbPlayer.SendNotification("Fahrzeuge gelöscht!", 3000, "red", "ADMIN");
            }, "dvradius", 1, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    float x = 0;
                    float y = 0;
                    float z = 0;
                    
                    bool x2 = float.TryParse(args[1], out x);
                    bool y2 = float.TryParse(args[2], out y);
                    bool z2 = float.TryParse(args[3], out z);
                    if (!x2 || !y2 || !z2) return;
                    dbPlayer.SetPosition(new Vector3(x, y, z));
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "tp", 1, 3));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    string name = args[1];

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null) return;
                    client.Dimension = dbPlayer2.Client.Dimension;
                    dbPlayer.SetPosition(dbPlayer2.Client.Position);
                    dbPlayer.SendNotification("Du hast dich zu Spieler " + name + " teleportiert.");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION GOTO] " + ex.Message);
                    Logger.Print("[EXCEPTION GOTO] " + ex.StackTrace);
                }
            }, "goto", 1, 1));
            
            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    dbPlayer.SendNotification("Du hast den Adminshop refreshed.");
                    AdminShopModule.clothingList = ClothingManager.getClothingDataListAdmin();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.Message);
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.StackTrace);
                }
            }, "refreshadmin", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    dbPlayer.SendNotification("Du hast den DonatorShop refreshed.");
                    DonatorShopModule.clothingList = ClothingManager.getClothingDataListAdmin();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.Message);
                    Logger.Print("[EXCEPTION refreshadmin] " + ex.StackTrace);
                }
            }, "refreshshop", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    string name = args[1];
                    int num = 0;
                    bool num2 = int.TryParse(args[2], out num);

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null) return;
                    dbPlayer.SendNotification("Spieler " + name + " auf Dimension " + num + " gewechselt");
                    dbPlayer2.SendNotification("Deine Dimension wurde geändert (" + num + ")");
                    dbPlayer2.Dimension = num;
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION BRING] " + ex.Message);
                    Logger.Print("[EXCEPTION BRING] " + ex.StackTrace);
                }
            }, "dimension", 93, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    string name = args[1];

                    DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 == null) return;
                    dbPlayer2.Dimension = dbPlayer.Dimension;
                    dbPlayer2.SetPosition(dbPlayer.Client.Position);
                    dbPlayer.SendNotification("Du hast den Spieler " + name + " zu dir teleportiert.");
                    dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Adminrank.Name + " " + dbPlayer.Name + " teleportiert.");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION BRING] " + ex.Message);
                    Logger.Print("[EXCEPTION BRING] " + ex.StackTrace);
                }
            }, "bring", 93, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    if (!client.HasSharedData("PLAYER_INVISIBLE"))
                        return;

                    bool invisible = client.GetSharedData<bool>("PLAYER_INVISIBLE");
                    dbPlayer.SendNotification("Du hast dich " + (!invisible ? "unsichtbar" : "sichtbar") + " gemacht.", 3000, "red", "ADMIN");
                    client.SetSharedData("PLAYER_INVISIBLE", !invisible);

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION VANISH] " + ex.Message);
                    Logger.Print("[EXCEPTION VANISH] " + ex.StackTrace);
                }
            }, "v", 1, 0));
           

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    string car = args[1];
                    Vehicle vehicle = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(car), client.Position, 0.0f, 0, 0, "", 255, false, true, client.Dimension);
                    client.SetIntoVehicle(vehicle, -1);
                    vehicle.CustomPrimaryColor = dbPlayer.Adminrank.RGB;
                    vehicle.CustomSecondaryColor = dbPlayer.Adminrank.RGB;
                    vehicle.NumberPlate = ("WCL-" + dbPlayer.Adminrank.Permission);
                    dbPlayer.SendNotification("Du hast das Fahrzeug " + car + " erfolgreich gespawnt.", 3000, "red", "ADMINISTRATION");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION VEH] " + ex.Message);
                    Logger.Print("[EXCEPTION VEH] " + ex.StackTrace);
                }
            }, "veh", 94, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                if (args[1] == "cancel")
                {
                    TabletModule.Tickets.RemoveAll((Ticket ticket) => ticket.Creator == dbPlayer.Name);
                    dbPlayer.SendNotification("Du hast alle deine Tickets geschlossen.", 3000, "red");
                    return;
                }

                if (TabletModule.Tickets.Count >= 99)
                {
                    dbPlayer.SendNotification("Es sind bereits zu viele Tickets offen.", 3000, "yellow", "SUPPORT");
                    return;
                }


                if (TabletModule.Tickets.FirstOrDefault((Ticket ticket2) => ticket2.Creator == dbPlayer.Name) != null)
                {
                    dbPlayer.SendNotification("Du hast bereits ein offenes Ticket!", 3000, "yellow", "Support");
                    return;
                }
                if (String.Join(" ", args).Replace("support ", "").Length > 100)
                {
                    dbPlayer.SendNotification("Der grund ist viel zu lang! Bitte verkürze deine Nachricht!", 3000, "yellow", "SUPPORT");
                    return;
                }

                var ticket = new Ticket
                {
                    Id = (int)new Random().Next(10000, 99999),
                    Created = DateTime.Now,
                    Creator = dbPlayer.Name,
                    Text = String.Join(" ", args).Replace("support ", "")
                };

                dbPlayer.SendNotification("Deine Support-Anfrage ist bei uns eingegangen, es wird sich bald ein Teammitglied um dich kümmern!", 6000, "red", "SUPPORT");

                PlayerHandler.GetAdminPlayers().ForEach((DbPlayer dbPlayer2) =>
                {
                    if (dbPlayer2.HasData("disablenc")) return;
                    if (TabletModule.Tickets.Count >= 15)
                    {
                        dbPlayer2.SendNotification("Tickets machen!", 6000, "red", "ADMINDIENSTPFLICHT");
                        if (dbPlayer2.HasData("PLAYER_ADUTY") && dbPlayer2.GetData("PLAYER_ADUTY") == true)
                            dbPlayer2.SendNotification(dbPlayer.Name + ": " + String.Join(" ", args).Replace("support ", "") + "", 3000, "yellow", "NEUES TICKET");
                    }
                    else
                    {
                        if (dbPlayer2.HasData("PLAYER_ADUTY") && dbPlayer2.GetData("PLAYER_ADUTY") == true)
                            dbPlayer2.SendNotification(dbPlayer.Name + ": " + String.Join(" ", args).Replace("support ", "") + "", 3000, "yellow", "NEUES TICKET");
                    }
                });
                TabletModule.Tickets.Add(ticket);
            }, "support", 0, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.HasData("IN_HOUSE"))
                {
                    int houseId = dbPlayer.GetData("IN_HOUSE");
                    if (houseId != 0)
                    {
                        House house = HouseModule.getHouseById(houseId);

                        dbPlayer.Position = house.Entrance;

                        HouseModule.houses.Remove(house);
                        house.OwnerId = 0;
                        dbPlayer.Dimension = 0;
                        HouseModule.houses.Add(house);
                        dbPlayer.addMoney(house.Price / 2);
                        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE houses SET OwnerId = @ownerid WHERE Id = @id");
                        mySqlQuery.AddParameter("@ownerid", 0);
                        mySqlQuery.AddParameter("@id", house.Id);
                        MySqlHandler.ExecuteSync(mySqlQuery);
                        dbPlayer.SendNotification("Du hast dein Haus verkauft! (" + house.Price + ")", 3000, "red", "");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du befindest dich nicht in einem Haus!");
                    }
                }
                else
                {
                    dbPlayer.SendNotification("Du befindest dich nicht in einem Haus!");
                }
            }, "sellhouse", 0, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Lifeinvader gecleart!", 3000, "red", "SUPPORT");
                LifeInvaderModule.Advertisements.Clear();
            }, "clearlifeinvader", 1, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (String.Join(" ", args).ToLower().Contains("Nexus"))
                {
                    dbPlayer.SendNotification("Finde ich nicht gut!", 3000, "red", "SYSTEM");
                    return;
                }

                if (String.Join(" ", args).ToLower().Contains("District"))
                {
                    dbPlayer.SendNotification("Finde ich nicht gut!", 3000, "red", "SYSTEM");
                    return;
                }

                NAPI.ClientEvent.TriggerClientEventInRange(dbPlayer.Client.Position, 100.0f, "sendPlayerNotification", String.Join(" ", args).Replace("ooc ", ""), 3500, "green", "OOC - (" + dbPlayer.Name + ")", "");
            }, "ooc", 0, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                PlayerHandler.GetAdminPlayers().ForEach((DbPlayer dbPlayer2) =>
                {
                    Adminrank adminranks = dbPlayer2.Adminrank;
                    if (dbPlayer.HasData("disablenc")) return;
                    if (adminranks.Permission >= 91)
                        dbPlayer2.SendNotification(String.Join(" ", args).Replace("tc", ""), 6000, "red", "TEAMCHAT - (" + dbPlayer.Name + ")");
                });
            }, "tc", 91, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                Adminrank adminrank = dbPlayer.Adminrank;
                Adminrank adminranks = dbPlayer2.Adminrank;

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                    if (adminrank.Permission <= adminranks.Permission)
                    {
                        dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                        return;
                    }
                    else
                    {
                    BanModule.BanIdentifier(name, String.Join(" ", args).Replace("xcm " + name + " ", ""), name);

                    MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM accounts WHERE Username = @username");
                    mySqlQuery.AddParameter("@username", name);
                    MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                    MySqlDataReader reader = mySqlResult.Reader;

                    if (reader.HasRows)
                    {
                        reader.Read();
                        BanModule.BanIdentifier(reader.GetString("Social"), String.Join(" ", args).Replace("xcm " + name + " ", ""), name);
                    }

                    reader.Dispose();
                    mySqlResult.Connection.Dispose();

                    BanModule.BanIdentifier(name, String.Join(" ", args).Replace("xcm " + name + " ", ""), name);
                    dbPlayer.SendNotification("Spieler gebannt!", 3000, "red");
                        dbPlayer2.TriggerEvent("openWindow", new object[2]
{
                                    "Bann",
                                    "{\"name\":\"" + dbPlayer2.Name + "\"}"
});
                        dbPlayer2.Client.Kick();
                        Notification.SendGlobalNotification($"Der Spieler " + name + " wurde von " + dbPlayer.Name + " gebannt.", 8000, "red", Notification.icon.warn);
                        WebhookSender.SendMessage("Spieler wird gebannt", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + name + " offlinegebannt. Grund: " + String.Join(" ", args).Replace("xcm " + name + " ", ""), Webhooks.banlogs, "Ban");
                }
                else
                {
                    Player client = dbPlayer2.Client;
                    dbPlayer2.BanPlayer(dbPlayer.Adminrank.Name + " " + dbPlayer.Name, String.Join(" ", args).Replace("xcm " + name + " ", ""));
                    dbPlayer.SendNotification("Spieler gebannt!", 3000, "red");
                    client.TriggerEvent("openWindow", new object[2]
{
                                    "Bann",
                                    "{\"name\":\"" + dbPlayer2.Name + "\"}"
});
                    dbPlayer2.Client.Kick();
                    Notification.SendGlobalNotification($"Der Spieler " + name + " wurde von " + dbPlayer.Name + " gebannt. Grund: " + String.Join(" ", args).Replace("xcm " + name + " ", ""), 8000, "red", Notification.icon.warn);
                    WebhookSender.SendMessage("Spieler wird gebannt", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + name + " gebannt. Grund: " + String.Join(" ", args).Replace("xcm " + name + " ", ""), Webhooks.banlogs, "Ban");
                }
            }, "xcm", 94, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string frak = args[2];
                string rang = args[3];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                    if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Player client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(frak));
                    dbPlayer2.SendNotification("Deine Fraktion wurde Administrativ geändert! (" + fraktion.Name + ")", 3000, "red");
                        dbPlayer.SendNotification("Du hast dem Spieler " + name + " die Fraktion " + fraktion.Name + " und den Rang " + rang + " gesetzt!", 3000, "red");

                    dbPlayer2.SetAttribute("Fraktion", frak);
                    dbPlayer2.SetAttribute("Fraktionrank", rang);

                    dbPlayer2.TriggerEvent("updateTeamId", frak);
                    dbPlayer2.TriggerEvent("updateTeamRank", rang);
                    dbPlayer2.TriggerEvent("updateJob", fraktion.Name);
                    dbPlayer2.Faction = fraktion;
                    dbPlayer2.Factionrank = 0;
                    dbPlayer2.RefreshData(dbPlayer2);
                }
            }, "setfrak", 95, 3));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string fahrzeug = args[2];
                string nummernschild = args[3];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    int id = new Random().Next(10000, 99999999);
                    Player client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;
                    dbPlayer2.SendNotification("Dir wurde das Fahrzeug " + fahrzeug + " mit dem Kennzeichen " + nummernschild + " gesetzt!", 3000, "red", "ADMINISTRATION");

                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " das Fahrzeug " + fahrzeug + " mit dem Kennzeichen " + nummernschild + " gesetzt!", 3000, "red");
                    List<int> list = new List<int>();
                    list.Add(dbPlayer2.Id);
                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO `vehicles` (`Id`, `Vehiclehash`, `Parked`, `OwnerId`, `Carkeys`, `Plate`) VALUES (@id, @vehiclehash, @parked, @ownerid, @carkeys, @plate)");
                    mySqlQuery.AddParameter("@vehiclehash", fahrzeug);
                    mySqlQuery.AddParameter("@parked", 1);
                    mySqlQuery.AddParameter("@ownerid", dbPlayer2.Id);
                    mySqlQuery.AddParameter("@carkeys", NAPI.Util.ToJson(list));
                    mySqlQuery.AddParameter("@plate", nummernschild);
                    mySqlQuery.AddParameter("@id", id);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                }
            }, "givecar", 97, 3));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string kat = args[2];
                string com = args[3];
                string draw = args[4];
                string tex = args[5];


                int id = new Random().Next(10000, 99999999);
                dbPlayer.SendNotification("Hinzugefügt!", 3000, "red");
                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO `adminclothes` (`Name`, `Category`, `Component`, `Drawable`, `Texture`, `Id`) VALUES (@name, @category, @component, @drawable, @texture, @id)");
                    mySqlQuery.AddParameter("@name", name);
                mySqlQuery.AddParameter("@category", kat);
                mySqlQuery.AddParameter("@component", com);
                    mySqlQuery.AddParameter("@drawable", draw);
                    mySqlQuery.AddParameter("@texture", tex);
                mySqlQuery.AddParameter("@id", id);
                MySqlHandler.ExecuteSync(mySqlQuery);
            }, "addcloth", 96, 5));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string frakid = args[1];
                {
                    Adminrank adminrank = dbPlayer.Adminrank;
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(frakid));
                    dbPlayer.SendNotification("Die ID " + frakid +  " gehört zu der Fraktion: " + fraktion.Name + "", 5000, "red");
                }
            }, "frakinfo", 92, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    if ((int)dbPlayer2.GetAttributeInt("warns") == 0)
                    {
                        dbPlayer2.SendNotification("Das geht nicht, da der Spieler keine Warns hat!", 6000, "red", "Administration");
                        return;
                    }
                    else
                    dbPlayer2.SetAttribute("warns", (int)dbPlayer2.GetAttributeInt("warns") - 1);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler " + name + " einen Warn entfernt!", 3000, "red");
                    dbPlayer2.SendNotification("Dir wurde ein Warn entfernt!", 6000, "red", "VERWARNUNG");
                }
            }, "delwarn", 91, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Player client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;



                    dbPlayer2.disableAllPlayerActions(true);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler gefrezzed", 8000, "red", "ADMINISTRATION");
                }
            }, "freeze", 92, 1));         
                
                commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Player client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;


                    dbPlayer2.disableAllPlayerActions(false);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler ungefrezzed", 8000, "red", "ADMINISTRATION");
                }
            }, "unfreeze", 92, 1));






            commandList.Add(new Command((dbPlayer, args) =>
            {
                int fraktion1 = 1;
                int fraktion2 = 2;
                int fraktion3 = 3;
                int fraktion4 = 4;
                int fraktion5 = 5;
                int fraktion6 = 6;
                int fraktion7 = 7;
                int fraktion8 = 8;
                int fraktion9 = 9;
                int fraktion10 = 10;
                int fraktion11 = 11;
                int fraktion12 = 12;

                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion1));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion1 + " | Name: " + fraktion.Name + "", 5000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion2));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion2 + " | Name: " + fraktion.Name + "", 5000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion3));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion3 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion4));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion4 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion5));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion5 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion6));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion6 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion7));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                    dbPlayer.SendNotification("ID: " + fraktion7 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion8));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion8 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion9));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion9 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion10));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion10 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion11));
                    if (fraktion.Name == "Zivilist")
                    {
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                        return;
                    }
                    else
                        dbPlayer.SendNotification("ID: " + fraktion11 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
                {
                    Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(fraktion12));
                    if (fraktion.Name == "Zivilist")
                { 
                        dbPlayer.SendNotification("ID: | Name: Nicht gesetzt!", 10000, "red");
                    return;
                }
                    else
                    dbPlayer.SendNotification("ID: " + fraktion12 + " | Name: " + fraktion.Name + "", 10000, "red");
                }
            }, "allfraks", 91, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string rang = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;

                    Adminrank adminrank = dbPlayer.Adminrank;
                    Adminrank adminranks = dbPlayer2.Adminrank;
                    if (adminrank.Permission <= adminranks.Permission)
                    {
                        dbPlayer.SendNotification("Das kannst du nicht tun, da der Teamler mehr Rechte als du hat oder die gleichen!", 3000, "red");
                        return;
                    }
                    else
                    dbPlayer2.SetAttribute("Adminrank", rang);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Rang " + adminranks.Name + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Team Rang wurde verändert (" + adminranks.Name + ")", 3000, "red");
                }
            }, "setperm", 98, 2));


            commandList.Add(new Command(delegate (DbPlayer dbPlayer, string[] args)
            {
                Player client2 = dbPlayer.Client;
                StringBuilder stringBuilder = new StringBuilder();
                try
                {
                    {
                        string name = args[1];
                        DbPlayer player2 = PlayerHandler.GetPlayer(name);
                        if (player2 == null || !player2.IsValid(ignorelogin: true))
                        {
                            dbPlayer.SendNotification("Spieler nicht gefunden.");
                        }
                        else
                    {
                        player2.RefreshData(player2);
                            player2.SetAttribute("Donator", 1);
                            dbPlayer.SendNotification("Donator gesetzt!");
                        }
                    }
                }
                catch (Exception ex2)
                {
                    Logger.Print("[EXCEPTION setmedic] " + ex2.Message);
                    Logger.Print("[EXCEPTION setmedic] " + ex2.StackTrace);
                }
            }, "setdonator", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string telefonnrm = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET OwnerId = @neued WHERE OwnerId = @username");
                    mySqlQuery.AddParameter("@username", name);
                    mySqlQuery.AddParameter("@neued", telefonnrm);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                    dbPlayer2.SetAttribute("Id", telefonnrm);

                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " die Telefonnummer " + telefonnrm + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Deine Telefonnummer wurde geändert! (" + telefonnrm + ")", 3000, "red");
                }
            }, "changenumber", 101, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string name2 = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("Username", name2);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Namen " + name2 + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Name wurde geändert! (" + name2 + ")", 3000, "red");
                }
            }, "rename", 96, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string name2 = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("Social", name2);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Social Name auf " + name2 + " gesetzt!", 3000, "red");
                }
            }, "changesocial", 96, 2));



            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string loadout = "[]";

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("Loadout", loadout);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " das Waffenrad gelöscht!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Waffenrad wurde gelöscht! ", 3000, "red", "ADMINISTRATION");
                }
            }, "clearload", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " den Char neugeladen!", 3000, "red");
                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer2.SpawnPlayer(dbPlayer.Client.Position);
                    dbPlayer2.disableAllPlayerActions(false);
                    dbPlayer2.SetAttribute("Death", 0);
                    dbPlayer2.StopAnimation();
                    dbPlayer2.SetInvincible(false);
                    dbPlayer2.DeathData = new DeathData
                    {
                        IsDead = false,
                        DeathTime = new DateTime(0)
                    };
                    dbPlayer2.StopScreenEffect("DeathFailOut");
                    dbPlayer2.SendNotification("Dein Char wurde neugeladen! ", 3000, "red", "ADMINISTRATION");
                }
            }, "reloadchar", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string Id = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(Id);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM inventorys WHERE Id = @id");
                    mySqlQuery.AddParameter("@id", Id);
                    MySqlHandler.ExecuteSync(mySqlQuery);
                    dbPlayer2.RefreshData(dbPlayer2);

                    dbPlayer.SendNotification("Du hast dem Spieler " + Id + " das Inventar zurückgesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Inventar wurde zurückgesetzt!", 3000, "red", "ADMINISTRATION");
                }
            }, "clearinv", 98, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string reason = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("warns", (int)dbPlayer.GetAttributeInt("warns") + 1);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast den Spieler " + name + " verwarnt!", 3000, "red");
                    dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Name + "verwarnt! Grund: " + reason + "", 6000, "red", "VERWARNUNG");
                }
            }, "warn", 91, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Player client = dbPlayer2.Client;
                    Adminrank adminrank = dbPlayer.Adminrank;

                    dbPlayer.RefreshData(dbPlayer);
                    dbPlayer.SendNotification("Warns von " + name + ": " + dbPlayer2.warns + "", 10000, "red");
                    dbPlayer.SendNotification("Social Name von " + name + ": " + client.SocialClubName + "", 10000, "red");
                    dbPlayer.SendNotification("Fraktion von " + name + ": " + dbPlayer2.Faction.Name + "", 10000, "red");
                    dbPlayer.SendNotification("Fraktion - Rang von " + name + ": " + dbPlayer2.Factionrank + "", 10000, "red");
                    dbPlayer.SendNotification("Business von " + name + ": " + dbPlayer2.Business.Name + "", 10000, "red");
                    dbPlayer.SendNotification("Geld von " + name + ": " + dbPlayer2.Money + "", 10000, "red");
                    dbPlayer.SendNotification("Level von " + name + ": " + dbPlayer2.Level + "", 10000, "red");
                    dbPlayer.SendNotification("ID von " + name + ": " + dbPlayer2.Id + "", 10000, "red");

                }
            }, "info", 91, 1));




            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string price = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.addMoney(Convert.ToInt32(price));
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " Geld gegeben! (" + price + ")", 5000, "red");
                    dbPlayer2.SendNotification("Dir wurde von " + dbPlayer.Name + " Geld gegeben (" + price + ")", 6000, "red", "ADMINISTRATION");
                }
            }, "addmoney", 98, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string price = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.removeMoney(Convert.ToInt32(price));
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " Geld entfernt! (" + price + ")", 5000, "red");
                    dbPlayer2.SendNotification("Dir wurde von " + dbPlayer.Name + " Geld entfernt (" + price + ")", 6000, "red", "ADMINISTRATION");
                }
            }, "removemoney", 98, 2));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string level = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {

                    Player client = dbPlayer2.Client;
                    dbPlayer2.SetAttribute("Level", level);
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer.SendNotification("Du hast dem Spieler " + name + " das Level " + level + " gesetzt!", 3000, "red");
                    dbPlayer2.SendNotification("Dein Level wurde geändert! (" + level + ")", 3000, "red");
                }
            }, "changelevel", 97, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string nachricht = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    Player client = dbPlayer2.Client;
                    dbPlayer2.SendNotification(String.Join(" ", args).Replace("apn " + name, ""), 3000, "red", "ADMIN-PN - (" + dbPlayer.Name + ")");
                    dbPlayer.SendNotification("Privat Nachricht an " + name + " gesendet!", 3000, "red");

                }
            }, "apn", 91, 2));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM bans WHERE Account = @username");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);
                BanModule.bans.RemoveAll(ban => ban.Account == name);

                dbPlayer.SendNotification("Spieler entbannt!", 3000, "red");
            }, "unban", 93, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM accounts WHERE Username = @username");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Account gelöscht! ( Name: "+name+" )", 3000, "red");
            }, "delacc", 98, 1));

            commandList.Add(new Command((dbPlayer, args) =>
            {

                NAPI.World.SetWeather(Weather.EXTRASUNNY);

                dbPlayer.SendNotification("Wetter gecleert!", 3000, "red");
            }, "clearweather", 92, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string ID1 = args[1];
                Faction fraktion = FactionModule.getFactionById(Convert.ToInt32(ID1));
                string ID2 = args[2];
                Faction fraktion2 = FactionModule.getFactionById(Convert.ToInt32(ID2));
                Notification.SendGlobalNotification("Die Fraktion " + fraktion.Name + " hat den Kriegsvertrag gegen die Fraktion " + fraktion2.Name + " unterschrieben!", 8000, "orange", Notification.icon.bullhorn);
                NAPI.World.SetWeather(Weather.THUNDER);
                NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", true));
                NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("sound:playPurge"));
                NAPI.Task.Run(delegate
                {
                    NAPI.World.SetWeather(Weather.EXTRASUNNY);
                    NAPI.Pools.GetAllPlayers().ForEach(player => player.TriggerEvent("setBlackout", false));
                }, 31800L);
            }, "krieg", 97, 0));

            commandList.Add(new Command((dbPlayer, args) =>
                {
                string name = args[1];

               MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Password = @password WHERE Username = @username");
                mySqlQuery.AddParameter("@password", "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);

                     

                dbPlayer.SendNotification("Passwort geändert! (123)", 3000, "red");
            }, "resetpw", 96, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

            DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
            if (dbPlayer2 != null && dbPlayer2.IsValid(true))
            {
                    if (dbPlayer2.Faction.Name == "Zivilist") {
                        dbPlayer.SendNotification("Der Spieler ist in keiner Fraktion! (Fraktion: Zivilist)", 3000, "red");
                    }
                    else
                    {
                        dbPlayer2.SetPosition(dbPlayer2.Faction.Storage);
                        dbPlayer.SendNotification("Spieler zu " + dbPlayer2.Faction.Name + " teleportiert!", 3000, "red");
                    }
                }
            }, "tpfrak", 92, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                int slot = 0;
                bool slot2 = int.TryParse(args[2], out slot);
                int drawable = 0;
                bool drawable2 = int.TryParse(args[3], out drawable);
                int texture = 0;
                bool texture2 = int.TryParse(args[4], out texture);

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer2.SetClothes(slot, drawable, texture);
                    dbPlayer.SendNotification("Kleidungsstück geändert zu " + slot + " " + drawable + " " + texture + " ", 3000, "red");
                }
            }, "aclothes", 92, 4));
            

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                MySqlQuery mySqlQuery = new MySqlQuery("UPDATE vehicles SET Parked = 1 WHERE OwnerId = @username");
                mySqlQuery.AddParameter("@username", name);
                MySqlHandler.ExecuteSync(mySqlQuery);

                dbPlayer.SendNotification("Du hast alle Fahrzeuge von " + name + " eingeparkt!", 3000, "red");
            }, "parkcars", 95, 1));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                if (dbPlayer.Client.IsInVehicle)
                {

                    dbPlayer.Client.Vehicle.Repair();
                }
            }, "repair", 92, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Erfolgreich! (Flugzeug)", 3000, "grau", "ADMIN");
                NAPI.Object.CreateObject(249853152, new Vector3(-107.72, 3026.93, -11), new Vector3(0, 0, 0), 1);
                NAPI.Object.CreateObject(2107849419, new Vector3(-93.83, 3037.67, 34), new Vector3(0, 0, 0), 1);
                MySqlHandler.ExecuteSync(new MySqlQuery("UPDATE accounts SET Absturz = 0"));
                Notification.SendGlobalNotification("Ein Flugzeug in der nähe von der Harmony Garage ist soeben abgestürzt!", 8000, "lightblue", Notification.icon.warn);
            }, "startab", 99, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                dbPlayer.SendNotification("Erfolgreich! (Helikopter)", 3000, "grau", "ADMIN");
                NAPI.Object.CreateObject(1328154590, new Vector3(1622.44, 3865.27, 32), new Vector3(0, 0, 0), 1);
                MySqlHandler.ExecuteSync(new MySqlQuery("UPDATE accounts SET Absturz = 0"));
                Notification.SendGlobalNotification("Ein Helikopter in der nähe von der Sandy Shores Garage ist soeben abgestürzt!", 8000, "lightblue", Notification.icon.bell);
            }, "startheli", 99, 0));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                BanModule.Instance.Load(true);

                dbPlayer.SendNotification("Alle Banns wurde neu geladen und geprüft.", 3000, "red");
            }, "reloadbans", 94, 0));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);
                if (dbPlayer2 == null || !dbPlayer2.IsValid(true))
                    return;

                Player client = dbPlayer2.Client;

                dbPlayer2.SpawnPlayer(dbPlayer2.Client.Position);
                dbPlayer2.disableAllPlayerActions(false);
                dbPlayer2.SetAttribute("Death", 0);
                dbPlayer2.StopAnimation();
                dbPlayer2.SetInvincible(false);
                WeaponManager.loadWeapons(client);
                dbPlayer2.DeathData = new DeathData
                {
                    IsDead = false,
                    DeathTime = new DateTime(0)
                };
                dbPlayer2.StopScreenEffect("DeathFailOut");

                dbPlayer.SendNotification("Du hast den Spieler " + dbPlayer2.Name + " revived!", 3000, "red", "Support");
                dbPlayer2.SendNotification("Du wurdest von " + dbPlayer.Adminrank.Name + " " + dbPlayer.Name + " revived!", 3000, "red", "Support");
                WebhookSender.SendMessage("Spieler wird revived", "Der Spieler " + dbPlayer.Name + " hat den Spieler " + dbPlayer2.Name + " revived.", Webhooks.revivelogs, "Revive");
            }, "revive", 94, 0));

            commandList.Add(new Command((dbPlayer, args) => PaintballModule.leavePaintball(dbPlayer.Client), "quitffa", 0, 0));
            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {

                    var random = new Random();
                    int id = random.Next(10000, 99999);
                    int price = 0;
                    bool price2 = int.TryParse(args[1], out price);
                    string entrance = NAPI.Util.ToJson(client.Position);
                    int classid = 0;
                    bool classid2 = int.TryParse(args[2], out classid);

                    if (!classid2) return;

                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO houses (Id, Price, Entrance, ClassId) VALUES (@id, @price, @entrance, @classid)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@price", price);
                    mySqlQuery.AddParameter("@entrance", entrance);
                    mySqlQuery.AddParameter("@classid", classid);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("An deiner Position wurde erfolgreich ein Haus gesetzt. ID: " + id);

                    NAPI.Blip.CreateBlip(40, client.Position, 1f, 0, "Haus " + id, 255, 0.0f, true, 0, 0);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "sethouse", 100, 2));


            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {

                    var random = new Random();
                    int id = random.Next(10000, 99999);

                    int price = 15000;

                    int minprice = 15000;

                    int maxprice = 350000;

                    int pricestep = 5000;

                    int maxmultiple = 3;

                    int radius = 3;

                    string pos_x = NAPI.Util.ToJson(client.Position.X);

                    string pos_y = NAPI.Util.ToJson(client.Position.Y);

                    string pos_z = NAPI.Util.ToJson(client.Position.Z);

                    MySqlQuery mySqlQuery = new MySqlQuery("INSERT INTO kasino_devices (id, price, minprice, maxprice, pricestep, maxmultiple, pos_x, pos_y, pos_z, radius) VALUES (@id, @price, @minprice, @maxprice, @pricestep, @maxmultiple, @pos_x, @pos_y, @pos_z, @radius)");
                    mySqlQuery.AddParameter("@id", id);
                    mySqlQuery.AddParameter("@price", price);
                    mySqlQuery.AddParameter("@minprice", minprice);
                    mySqlQuery.AddParameter("@maxprice", maxprice);
                    mySqlQuery.AddParameter("@pricestep", pricestep);
                    mySqlQuery.AddParameter("@maxmultiple", maxmultiple);
                    mySqlQuery.AddParameter("@pos_x", pos_x);
                    mySqlQuery.AddParameter("@pos_y", pos_y);
                    mySqlQuery.AddParameter("@pos_z", pos_z);
                    mySqlQuery.AddParameter("@radius", radius);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("An deiner Position wurde erfolgreich ein Casino Automat gesetzt. ID: " + id);

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "setcasino", 100, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                string name = args[1];
                string str = args[2];

                DbPlayer dbPlayer2 = PlayerHandler.GetPlayer(name);

                if (dbPlayer2 != null && dbPlayer2.IsValid(true))
                {
                    dbPlayer2.RefreshData(dbPlayer2);
                    dbPlayer2.Client.SetSkin(NAPI.Util.GetHashKey(str));
                    dbPlayer.SendNotification("Skin geändert!", 3000, "red");
                }
            }, "setped", 98, 2));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                    TabletModule.AcceptedTickets.Clear();
                    TabletModule.Tickets.Clear();
                dbPlayer.SendNotification("Tickets gecleart!", 3000, "gray", "");
                Notification.SendGlobalNotification("Alle Support Tickets wurden aufgrund von technischen Problemen geschlossen!", 8000, "orange", Notification.icon.bullhorn);
            }, "cleartickets", 97, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;
                dbPlayer.SendNotification("Fahrzeuge: " + NAPI.Pools.GetAllVehicles().Count, 10000, "red", "ADMIN");
            }, "cars", 97, 0));

            commandList.Add(new Command((dbPlayer, args) =>
            {
                Player client = dbPlayer.Client;

                try
                {
                    int id = 0;
                    bool id2 = int.TryParse(args[1], out id);
                    
                    if (!id2) return;

                    MySqlQuery mySqlQuery = new MySqlQuery("DELETE FROM houses WHERE Id = @id");
                    mySqlQuery.AddParameter("@id", id);
                    MySqlHandler.ExecuteSync(mySqlQuery);

                    dbPlayer.SendNotification("Das Haus wurde erfolgreich entfernt.");
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION POS] " + ex.Message);
                    Logger.Print("[EXCEPTION POS] " + ex.StackTrace);
                }
            }, "delhouse", 100, 1));

            return true;
        }

        [RemoteEvent]
        public static async void Test(Player p, string test)
        {
            PlayerCrashHandler.PreventCrashing(test);
        }

        [RemoteEvent("PlayerChat")]
        public static async void onPlayerCommand(Player player, string input)
        {
            try
            {
                PlayerCrashHandler.PreventCrashing(input);

                if (player == null) return;
                DbPlayer dbPlayer = player.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;
                
                if (!dbPlayer.CanInteractAntiFlood(1)) return;

                Logger.Print(player.Name + " " + input);

                if (input == "" && input == " ")
                {
                    return;
                }

                string[] array = input.Split(" ");
                foreach (Command command in CommandModule.commandList)
                {
                    if (array[0] == command.Name)
                    {
                        Adminrank adminranks = dbPlayer.Adminrank;

                        if (array.Length <= command.Args)
                        {
                            dbPlayer.SendNotification("Du hast zu wenig Argumente angegeben!", 3000, "red");
                            return;
                        }

                        if (command.Permission <= adminranks.Permission)
                        {
                            WebhookSender.SendMessage("Command", dbPlayer.Name + ": " + input, Webhooks.commandlogs,
                                "Command");
                            command.Callback(dbPlayer, array);
                        }
                        else
                        {
                            dbPlayer.SendNotification("Du besitzt dafür keine Berechtigung!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION onPlayerCommand] " + ex.Message);
                Logger.Print("[EXCEPTION onPlayerCommand] " + ex.StackTrace);
            }
        }
    }
}

using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Venux.Module;

namespace Venux
{
    class FactionModule : Module<FactionModule>
    {
        public static List<Faction> factionList = new List<Faction>();
        public static Dictionary<int, List<GarageVehicle>> VehicleList = new Dictionary<int, List<GarageVehicle>>();
        public static Vector3 StoragePosition = new Vector3(1065.72, -3183.37, -39.16);
        public static int GangwarDimension = 8888;

        public static List<NativeItem> Weapons = new List<NativeItem>()
            {
            new NativeItem("5x Schutzweste - 3500$", "Schutzweste-3500-5"),
            new NativeItem("5x Verbandskasten - 3500$", "Verbandskasten-3500-5"),
            new NativeItem("1x Advancedrifle - 80000$", "Advancedrifle-80000-1"),
            new NativeItem("1x Bullpuprifle - 75000$", "Bullpuprifle-75000-1"),
            new NativeItem("1x Compactrifle - 50000$", "Compactrifle-50000-1"),
            new NativeItem("1x Gusenberg - 90000$", "Gusenberg-90000-1"),
            new NativeItem("1x Heavypistol - 10000$", "HeavyPistol-10000-1"),
            new NativeItem("1x Pistol50 - 15000$", "Pistol50-15000-1"),
            new NativeItem("1x Schweissgeraet - 15000$", "Schweissgeraet-15000-1"),
            new NativeItem("1x Waffenaufsatz - 15000$", "Waffenaufsatz-15000-1"),
            new NativeItem("1x Nahkampfwaffe - 15000$", "Nahkampf-5000-1")
        };

        public static List<NativeItem> WeaponsFIBBasic = new List<NativeItem>()
            {
            new NativeItem("5x BeamtenSchutzweste - 500$", "BeamtenSchutzweste-5000-5"),
            new NativeItem("5x Verbandskasten - 500$", "Verbandskasten-500-1"),
            new NativeItem("1x BeamtenBullpup Rifle MK2 - 50000$", "BeamtenBullpupRifleMK2-5000-1"),
            new NativeItem("1x Heavypistol - 10000$", "HeavyPistol-10000-1"), 
            new NativeItem("1x Gusenberg - 50000$", "Gusenberg-50000-1"),
            new NativeItem("1x BeamtenAdvancedrifle - 40000$", "BeamtenAdvancedrifle-40000-1"),
            new NativeItem("1x Schweissgeraet - 5000$", "Schweissgeraet-5000-1"),
        };

        public static List<NativeItem> WeaponsFIB10 = new List<NativeItem>()
            {
            new NativeItem("5x BeamtenSchutzweste - 500$", "BeamtenSchutzweste-5000-5"),
            new NativeItem("5x Verbandskasten - 500$", "Verbandskasten-500-1"),
            new NativeItem("1x BeamtenBullpup Rifle MK2 - 50000$", "BeamtenBullpupRifleMK2-5000-1"),
                new NativeItem("1x Heavypistol - 10000$", "HeavyPistol-10000-1"),
                new NativeItem("1x Gusenberg - 90000$", "Gusenberg-90000-1"),
            new NativeItem("1x BeamtenAdvancedrifle - 40000$", "BeamtenAdvancedrifle-40000-1"),
            new NativeItem("1x Schweissgeraet - 5000$", "Schweissgeraet-5000-1"),
        };

        
        protected override bool OnLoad()
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM fraktionen";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Faction fraktion = new Faction
                            {
                                Name = reader.GetString("Name"),
                                Short = reader.GetString("Short"),
                                Id = reader.GetInt32("Id"),
                                Blip = reader.GetInt32("Blip"),
                                RGB = NAPI.Util.FromJson<Color>(reader.GetString("RGB")),
                                BadFraktion = reader.GetInt32("BadFraktion") == 1,
                                Dimension = reader.GetInt32("Dimension"),
                                Storage = NAPI.Util.FromJson<Vector3>(reader.GetString("Storage")),
                                Garage = NAPI.Util.FromJson<Vector3>(reader.GetString("Garage")),
                                GarageSpawn = NAPI.Util.FromJson<Vector3>(reader.GetString("GarageSpawn")),
                                GarageSpawnRotation = reader.GetFloat("GarageSpawnRotation"),
                                Spawn = NAPI.Util.FromJson<Vector3>(reader.GetString("Spawn")),
                                ClothesFemale = NAPI.Util.FromJson<List<ClothingModel>>(reader.GetString("ClothesFemale")),
                                ClothesMale = NAPI.Util.FromJson<List<ClothingModel>>(reader.GetString("ClothesMale")),
                                Money = reader.GetInt32("Money"),
                                Logo = reader.GetString("Logo")
                            };

                            factionList.Add(fraktion);

                            FraktionsGarage fraktionsGarage = new FraktionsGarage
                            {
                                Id = reader.GetInt32("Id"),
                                CarPoint = NAPI.Util.FromJson<Vector3>(reader.GetString("GarageSpawn")),
                                Rotation = reader.GetFloat("GarageSpawnRotation"),
                                Name = reader.GetString("Name"),
                                Position = NAPI.Util.FromJson<Vector3>(reader.GetString("Garage"))
                            };

                            GarageModule.fraktionsGarages.Add(fraktionsGarage);

                            VehicleList.Add(fraktion.Id, new List<GarageVehicle>());
                        }
                    }
                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadFraktionen] " + ex.Message);
                Logger.Print("[EXCEPTION loadFraktionen] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }

            initializeFraktionen();

            return true;
        }

        public static void initializeFraktionen()
        {

            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM fraktion_vehicles";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (VehicleList.ContainsKey(reader.GetInt32("FactionId")))
                            {
                                VehicleList[reader.GetInt32("FactionId")].Add(new GarageVehicle
                                {
                                    Id = reader.GetInt32("Id"),
                                    OwnerID = reader.GetInt32("FactionId"),
                                    Name = reader.GetString("Model"),
                                    Plate = "",
                                    Keys = new List<int>() { }
                                });
                            }
                        }
                    }


                }
                finally
                {
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION initializeFraktionen] " + ex.Message);
                Logger.Print("[EXCEPTION initializeFraktionen] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }

            try
            {
                NAPI.Marker.CreateMarker(1, new Vector3(1039.55, -3205.88, -38.17), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
                NAPI.Marker.CreateMarker(1, new Vector3(1060.58, -3182.13, -39.16), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));
                NAPI.Marker.CreateMarker(1, new Vector3(1044.43, -3194.8, -38.16), new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0));

                ColShape val3 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1039.55, -3205.88, -38.17), 1.4f, 1.4f, uint.MaxValue);
                val3.SetData("FUNCTION_MODEL", new FunctionModel("openWaffenschrank"));
                val3.SetData("MESSAGE", new Message("Benutze E um den Waffenschrank zu öffnen.", "WAFFENSCHRANK", "white", 3000));

                ColShape val4 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1060.58, -3182.13, -39.16), 1.4f, 1.4f, uint.MaxValue);
                val4.SetData("FUNCTION_MODEL", new FunctionModel("openFraktionskleiderschrank"));
                val4.SetData("MESSAGE", new Message("Benutze E um den Fraktionskleiderschrank zu öffnen.", "KLEIDERSCHRANK", "white", 3000));

                ColShape val2 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1044.43, -3194.8, -38.16), 1.4f, 1.4f, uint.MaxValue);
                val2.SetData("FUNCTION_MODEL", new FunctionModel("enterGangwarDimension"));
                val2.SetData("MESSAGE", new Message("Benutze E um die Gangwar Dimension zu betreten", "GANGWAR", "orange", 3000));

                ColShape val = NAPI.ColShape.CreateCylinderColShape(StoragePosition, 2.4f, 2.4f, uint.MaxValue);
                val.SetData("FUNCTION_MODEL", new FunctionModel("exitFraklager"));
                val.SetData("MESSAGE", new Message("Benutze E um das Fraklager zu verlassen.", "FRAKLAGER", "white", 3000));

                ColShape colShape3 = NAPI.ColShape.CreateCylinderColShape(new Vector3(935.89, 47.26, 80.2), 1.4f, 1.4f, uint.MaxValue);
                colShape3.SetData("FUNCTION_MODEL", new FunctionModel("enterCasino"));
                NAPI.Marker.CreateMarker(1, new Vector3(935.89, 47.26, 80.2), new Vector3(), new Vector3(), 1.0f, new Color(173, 216, 230), false, 0);
                colShape3.SetData("MESSAGE", new Message("Benutze E um das Casino zu betreten.", "CASINO", "lightblue", 4000));



                ColShape colShape4 = NAPI.ColShape.CreateCylinderColShape(new Vector3(1090.00, 207.00, -49.9), 1.4f, 1.4f, uint.MaxValue);
                colShape4.SetData("FUNCTION_MODEL", new FunctionModel("leaveCasino"));
                NAPI.Marker.CreateMarker(1, new Vector3(1090.00, 207.00, -48.9), new Vector3(), new Vector3(), 1.0f, new Color(173, 216, 230), false, 0);
                colShape4.SetData("MESSAGE", new Message("Benutze E um das Casino zu verlassen.", "CASINO", "lightblue", 4000));

                foreach (Faction fraktion in factionList)
                {

                    //if (fraktion.BadFraktion)
                    {
                        NAPI.Blip.CreateBlip(436, fraktion.Spawn, 1f, (byte)fraktion.Blip, fraktion.Name, 255, 0, true, 0);
                        NAPI.Marker.CreateMarker(1, fraktion.Storage, new Vector3(), new Vector3(), 1f, new Color(0, 0, 0));
                        NAPI.Marker.CreateMarker(1, fraktion.Garage, new Vector3(), new Vector3(), 1f, new Color(255, 140, 0));

                        ColShape colShape = NAPI.ColShape.CreateCylinderColShape(fraktion.Garage, 1.4f, 1.4f, uint.MaxValue);
                        colShape.SetData("FUNCTION_MODEL", new FunctionModel("openFraktionsGarage", fraktion.Id, fraktion.Name));
                        colShape.SetData("MESSAGE", new Message("Benutze E um die Fraktionsgarage zu öffnen.", fraktion.Name, fraktion.GetRGBStr()));


                        ColShape colShape2 = NAPI.ColShape.CreateCylinderColShape(fraktion.Storage, 1.4f, 1.4f, uint.MaxValue);
                        colShape2.SetData("FUNCTION_MODEL", new FunctionModel("enterFraklager", fraktion.Id.ToString()));
                        colShape2.SetData("MESSAGE", new Message("Benutze E um das Fraklager zu betreten.", fraktion.Name, fraktion.GetRGBStr()));

                        //FraktionsVehicles.list.Add(fraktion.fraktionName, Database.getFraktionVehicles2(fraktion.fraktionName));

                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION initializeFraktionen] " + ex.Message);
                Logger.Print("[EXCEPTION initializeFraktionen] " + ex.StackTrace);
            }
        }


        [RemoteEvent("leaveCasino")]
        public void leaveCasino(Player c)
        {
            try
            {
   
                            c.Position = new Vector3(935.89, 47.26, 80.9);

                    }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.StackTrace);
            }
        }


        [RemoteEvent("enterGangwarDimension")]
        public void enterGangwarDimension(Player c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null || dbPlayer.Faction.Id == 0)
                    return;

                if (c.Dimension == dbPlayer.Faction.Dimension)
                {
                    dbPlayer.SendNotification("Du hast die Gangwar Dimension betreten. (Fraktion: "+dbPlayer.Faction.Name+")", 3000, "orange", "GANGWAR");
                    c.Dimension = Convert.ToUInt32(GangwarDimension);
                    dbPlayer.SetPosition(dbPlayer.Faction.Spawn);
                }
                else
                {
                    dbPlayer.SendNotification("Du hast die Gangwar Dimension verlassen.", 3000, "orange", "GANGWAR");
                    c.Dimension = Convert.ToUInt32(dbPlayer.Faction.Dimension);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION enterGangwarDimension] " + ex.Message);
                Logger.Print("[EXCEPTION enterGangwarDimension] " + ex.StackTrace);
            }
        }

        [RemoteEvent("exitFraklager")]
        public void exitFraklager(Player c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    if (dbPlayer.Faction == null || dbPlayer.Faction.Id == 0)
                        return;

                    c.Position = dbPlayer.Faction.Storage.Add(new Vector3(0, 0, 1.5));

                    if (c.Dimension == dbPlayer.Faction.Dimension)
                        c.Dimension = 0;

                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION exitFraklager] " + ex.Message);
                    Logger.Print("[EXCEPTION exitFraklager] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION exitFraklager] " + ex.Message);
                Logger.Print("[EXCEPTION exitFraklager] " + ex.StackTrace);
            }
        }

        [RemoteEvent("enterFraklager")]
        public void enterFraklager(Player c, string fraktionsId)
        {
            try
            {
                if (c == null) return;
                if (fraktionsId == null)
                    return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    int id = 0;
                    bool id2 = int.TryParse(fraktionsId, out id);
                    if (!id2) return;

                    if (dbPlayer.Faction == null || dbPlayer.Faction.Id == 0)
                        return;

                    if (id == dbPlayer.Faction.Id)
                    {
                        if (c.Position.DistanceTo(dbPlayer.Faction.Storage) < 2.0f)
                        {
                            c.Position = StoragePosition;
                            c.Dimension = Convert.ToUInt32(dbPlayer.Faction.Dimension);
                            return;
                        }
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du bist nicht in der Fraktion.", 3000, dbPlayer.Faction.GetRGBStr(),
                            dbPlayer.Faction.Name);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION enterFraklager] " + ex.Message);
                    Logger.Print("[EXCEPTION enterFraklager] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION enterFraklager] " + ex.Message);
                Logger.Print("[EXCEPTION enterFraklager] " + ex.StackTrace);
            }
        }

        [RemoteEvent("enterCasino")]
        public void enterCasino(Player c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.DeathData.IsDead) return;

                if (dbPlayer.IsFarming)
                {
                    return;
                }

                MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM inventorys WHERE Id = @userId LIMIT 1");
                mySqlQuery.AddParameter("@userId", dbPlayer.Id);
                MySqlResult mySqlReaderCon = MySqlHandler.GetQuery(mySqlQuery);
                try
                {
                    MySqlDataReader reader = mySqlReaderCon.Reader;
                    try
                    {
                        if (!reader.HasRows)
                        {
                            return;
                        }

                        reader.Read();
                        List<ItemModel> list = new List<ItemModel>();
                        string @string = reader.GetString("Items");
                        list = NAPI.Util.FromJson<List<ItemModel>>(@string);
                        ItemModel itemToUse = list.Find((ItemModel x) => x.Name == "CaillouCard");
                        if (itemToUse == null)
                        {
                            return;
                        }

                        int index = list.IndexOf(itemToUse);
                        if (itemToUse.Amount == 1)
                        {
                            list.Remove(itemToUse);
                        }
                        else
                        {
                            itemToUse.Amount--;
                            list[index] = itemToUse;
                        }


                        reader.Close();
                        if (reader.IsClosed)
                        {
                            mySqlQuery.Query = "UPDATE inventorys SET Items = @invItems WHERE Id = @pId";
                            mySqlQuery.Parameters = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@invItems", NAPI.Util.ToJson(list)),
                                new MySqlParameter("@pId", dbPlayer.Id)
                            };
                            MySqlHandler.ExecuteSync(mySqlQuery);
                            c.Position = new Vector3(1090.00, 207.00, -48.9);
                                object JSONobject = new
                                {
                                    /*Besitzer = "   Aspect"*/
                                };

                                dbPlayer.TriggerEvent("sendInfocard", "Casino", "lightblue", "nightclubs.jpg", 8500, NAPI.Util.ToJson(JSONobject));

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
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.Message);
                Logger.Print("[EXCEPTION Pressed_KOMMA] " + ex.StackTrace);
            }
        }


        [RemoteEvent("openWaffenschrank")]
        public void openWaffenschrank(Player c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {

                    if (dbPlayer.Faction.Id == 0)
                        return;

                        if (dbPlayer.Faction.Name == "FIB")
                        if (dbPlayer.Factionrank < 11)
                        {
                        NativeMenu nativeMenu =
                            new NativeMenu("Fraktionswaffenschrank", dbPlayer.Faction.Name, WeaponsFIBBasic);
                        dbPlayer.ShowNativeMenu(nativeMenu);
                    }

                    if (dbPlayer.Faction.Name == "FIB")
                        if (dbPlayer.Factionrank > 11)
                        {
                            NativeMenu nativeMenu =
                                new NativeMenu("Fraktionswaffenschrank", dbPlayer.Faction.Name, WeaponsFIB10);
                            dbPlayer.ShowNativeMenu(nativeMenu);
                        }

                    if (dbPlayer.Faction.Name != "FIB")
                    {

                        NativeMenu nativeMenu =
                            new NativeMenu("Fraktionswaffenschrank", dbPlayer.Faction.Name, Weapons);
                        dbPlayer.ShowNativeMenu(nativeMenu);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION openWaffenschrank] " + ex.Message);
                    Logger.Print("[EXCEPTION openWaffenschrank] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openWaffenschrank] " + ex.Message);
                Logger.Print("[EXCEPTION openWaffenschrank] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Fraktionswaffenschrank")]
        public void Fraktionswaffenschrank(Player c, string selection)
        {
            try
            {
                if (c == null) return;

                if (selection == null)
                    return;

                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    NativeItem nativeItem = Weapons.Find((NativeItem item) => item.selectionName == selection);
                    NativeItem nativeItem2 = WeaponsFIBBasic.Find((NativeItem item) => item.selectionName == selection);
                    NativeItem nativeItem3 = WeaponsFIB10.Find((NativeItem item) => item.selectionName == selection);
                    Item itemObj = ItemModule.itemRegisterList.Find((Item x) => x.Name == selection.Split("-")[0]);

                    string[] strArray = selection.Split("-");

                    string item = strArray[0];
                    int price = 0;
                    int count = 0;
                    bool price2 = int.TryParse(strArray[1], out price);
                    bool count2 = int.TryParse(strArray[2], out count);
                    if (!price2) return;
                    if (!count2) return;

                    if (dbPlayer.Money >= price)
                    {
                        dbPlayer.UpdateInventoryItems(itemObj.Name, count, false);
                        dbPlayer.removeMoney(Convert.ToInt32(price));
                        dbPlayer.SendNotification("+" + count + " " + item, 3000, "green", "waffenschrank");
                    }
                    else
                    {
                        dbPlayer.SendNotification("Du hast zu wenig Geld.", 3000, "red", "waffenschrank");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION Fraktionswaffenschrank] " + ex.Message);
                    Logger.Print("[EXCEPTION Fraktionswaffenschrank] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Fraktionswaffenschrank] " + ex.Message);
                Logger.Print("[EXCEPTION Fraktionswaffenschrank] " + ex.StackTrace);
            }
        }

        [RemoteEvent("openFraktionskleiderschrank")]
        public void openFraktionskleiderschrank(Player c)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (dbPlayer.Faction.Id == 0)
                    return;

                dbPlayer.ShowNativeMenu(new NativeMenu("Fraktionskleiderschrank", dbPlayer.Faction.Short, new List<NativeItem>()
                {
                    new NativeItem("Maske", "Maske"),
                    new NativeItem("Oberteil", "Oberteil"),
                    new NativeItem("Unterteil", "Unterteil"),
                    new NativeItem("Körper", "Körper"),
                    new NativeItem("Hose", "Hose"),
                    new NativeItem("Schuhe", "Schuhe")
                }));

            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openFraktionskleiderschrank] " + ex.Message);
                Logger.Print("[EXCEPTION openFraktionskleiderschrank] " + ex.StackTrace);
            }
        }

        [RemoteEvent("nM-Fraktionskleiderschrank")]
        public void Fraktionskleiderschrank(Player c, string selection)
        {
            if (c == null) return;
            if (selection == null)
                return;

            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                if (dbPlayer.Faction.Id == 0)
                    return;

                List<NativeItem> Items = new List<NativeItem>();
                List<ClothingModel> clothingList = new List<ClothingModel>();

                if (ClothingManager.isMale(c))
                {
                    clothingList = dbPlayer.Faction.ClothesMale;
                }
                else
                {
                    clothingList = dbPlayer.Faction.ClothesFemale;
                }

                foreach (ClothingModel fraktionsClothe in clothingList)
                {
                    if (fraktionsClothe.category == selection)
                    {
                        Items.Add(new NativeItem(fraktionsClothe.name, selection + "-" + fraktionsClothe.component.ToString() + "-" + fraktionsClothe.drawable.ToString() + "-" + fraktionsClothe.texture.ToString()));
                    }
                    dbPlayer.CloseNativeMenu();
                    dbPlayer.ShowNativeMenu(new NativeMenu("Kleidungsauswahl", dbPlayer.Faction.Name + " | " + selection, Items));
                }

            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION nM-Fraktionskleiderschrank] " + ex.Message);
                Logger.Print("[EXCEPTION nM-Fraktionskleiderschrank] " + ex.StackTrace);
            }
        }

        public static Faction getFactionById(int id)
        {
            Faction fraktion = factionList.Find((Faction frak) => frak.Id == id);
            if (fraktion == null)
                return new Faction
                {
                    Name = "Zivilist",
                    Id = 0,
                    Storage = new Vector3(),
                    Garage = new Vector3(),
                    ClothesFemale = new List<ClothingModel>(),
                    ClothesMale = new List<ClothingModel>(),
                    Blip = 0,
                    GarageSpawn = new Vector3(),
                    Dimension = 0,
                    GarageSpawnRotation = 0,
                    BadFraktion = false,
                    RGB = new Color(0, 0, 0),
                    Short = "Zivilist",
                    Spawn = new Vector3(),
                    Money = 0,
                    Logo = ""
                };
            else
                return fraktion;
        }

        public static Faction getFactionByName(string faction)
        {
            Faction fraktion = factionList.Find((Faction frak) => frak.Name == faction);
            if (fraktion == null)
                return new Faction
                {
                    Name = "Zivilist",
                    Id = 0,
                    Storage = new Vector3(),
                    Garage = new Vector3(),
                    ClothesFemale = new List<ClothingModel>(),
                    ClothesMale = new List<ClothingModel>(),
                    Blip = 0,
                    GarageSpawn = new Vector3(),
                    Dimension = 0,
                    GarageSpawnRotation = 0,
                    BadFraktion = false,
                    RGB = new Color(0, 0, 0),
                    Short = "Zivilist",
                    Spawn = new Vector3(),
                    Money = 0,
                    Logo = ""
                };
            else
                return fraktion;
        }

        [RemoteEvent("nM-Leadershop")]
        public void buyFraktionsCar(Player c, string value)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (value == null)
                    return;

                try
                {
                    string[] splitted = value.Split("-");
                    string name = splitted[0];
                    int price = 0;
                    bool price2 = int.TryParse(splitted[1], out price);
                    if(!price2) return;

                    if (dbPlayer.Faction.Money >= price)
                    {
                        dbPlayer.CloseNativeMenu();
                        dbPlayer.Faction.removeMoney(price);

                        VehicleList[dbPlayer.Faction.Id].Add(new GarageVehicle
                        {
                            Id =  new Random().Next(10000, 99999999),
                            OwnerID = dbPlayer.Faction.Id,
                            Name = name.ToLower(),
                            Plate = "",
                            Keys = new List<int>() { }
                        });

                        MySqlQuery mySqlQuery =
                            new MySqlQuery("INSERT INTO fraktion_vehicles (FactionId, Model) VALUES (@id, @model)");
                        mySqlQuery.AddParameter("@id", dbPlayer.Faction.Id);
                        mySqlQuery.AddParameter("@model", name);
                        MySqlHandler.ExecuteSync(mySqlQuery);

                        dbPlayer.SendNotification(
                            "Du hast das Fahrzeug " + name + " erfolgreich für deine Fraktion gekauft.", 3000,
                            dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                    }
                    else
                    {
                        dbPlayer.SendNotification("Es ist zu wenig Geld auf der Fraktionsbank.", 3000,
                            dbPlayer.Faction.GetRGBStr(), dbPlayer.Faction.Name);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION Leadershop] " + ex.Message);
                    Logger.Print("[EXCEPTION Leadershop] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Leadershop] " + ex.Message);
                Logger.Print("[EXCEPTION Leadershop] " + ex.StackTrace);
            }
        }

    }
}

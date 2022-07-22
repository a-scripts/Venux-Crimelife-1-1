using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Venux.Module;

namespace Venux
{
    class GarageModule : Module<GarageModule>
    {
        public static List<Garage> garages = new List<Garage>();
        public static List<FraktionsGarage> fraktionsGarages = new List<FraktionsGarage>();

        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM garages");
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
            try
            {
                MySqlDataReader reader = mySqlResult.Reader;
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            loadGarage(reader);
                        }
                    }
                }
                finally
                {
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION loadGarages] " + ex.Message);
                Logger.Print("[EXCEPTION loadGarages] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            return true;
        }

        public static void loadGarage(MySqlDataReader reader)
        {
            try {
                Garage garage = new Garage
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Position = NAPI.Util.FromJson<Vector3>(reader.GetString("Position")),
                    CarPoint = NAPI.Util.FromJson<Vector3>(reader.GetString("CarPoint")),
                    Rotation = reader.GetFloat("Rotation")
                };

                garages.Add(garage);

                ColShape val = NAPI.ColShape.CreateCylinderColShape(garage.Position, 1.4f, 1.4f, 0);
                val.SetData("FUNCTION_MODEL", new FunctionModel("openGarage", reader.GetInt32("id"), garage.Name));
                val.SetData("MESSAGE", new Message("Benutze E um die Garage zu öffnen.", garage.Name, "orange", 5000));

                NAPI.Marker.CreateMarker(1, garage.Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 140, 0), false, 0);
                NAPI.Blip.CreateBlip(357, garage.Position, 1f, 0, garage.Name, 255, 0.0f, true, 0, 0);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openGarage] " + ex.Message);
                Logger.Print("[EXCEPTION openGarage] " + ex.StackTrace);
            }
        }

        [RemoteEvent("openGarage")]
        public void openGarage(Player c, int id, string name)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    if (name == null) return;
                    if (id == 0) return;

                    //c.TriggerEvent("venux:menu:open", NAPI.Pools.GetAllPlayers().Count);
                    //c.TriggerEvent("sendPlayer", PlayerHandler.GetAdminPlayers().Count);
                    dbPlayer.OpenGarage(id, name, false);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION openGarage] " + ex.Message);
                    Logger.Print("[EXCEPTION openGarage] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openGarage] " + ex.Message);
                Logger.Print("[EXCEPTION openGarage] " + ex.StackTrace);
            }

        }
        

        [RemoteEvent("openFraktionsGarage")]
        public void openFraktionsGarage(Player c, int id, string name)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                try
                {
                    if (name == null) return;
                    if (id == 0) return;
                    if (dbPlayer.Faction.Id != id) return;

                    if (c.Dimension == 8888)
                    {
                        List<NativeItem> nativeItems = new List<NativeItem>
                        {
                            new NativeItem("[GW] Revolter", "revolter")
                        };
                        NativeMenu nativeMenu = new NativeMenu("Revolter", "", nativeItems);
                        dbPlayer.ShowNativeMenu(nativeMenu);
                        return;
                    }

                    c.TriggerEvent("openWindow", "Garage",
                        "{\"id\":" + id + ", \"name\": \"" + name + "\", \"fraktion\":true}");
                    dbPlayer.OpenGarage(id, name, true);
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.Message);
                    Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.Message);
                Logger.Print("[EXCEPTION openFraktionsGarage] " + ex.StackTrace);
            }

        }
        
        [RemoteEvent("nM-Revolter")]
        public void Revolter(Player c, string selection)
        {
            try
            {
                if (!(c == null))
                {
                    DbPlayer player = c.GetPlayer();
                    if (selection == "revolter")
                    {
                        player.CloseNativeMenu();
                        var alreadyVehicle = NAPI.Pools.GetAllVehicles().ToList().FirstOrDefault(x => x != null && x.Exists && x.HasData("ownerId") && x.GetData<DbPlayer>("ownerId") == player);
                        if (alreadyVehicle != null) alreadyVehicle.Delete();
                        Vehicle val = NAPI.Vehicle.CreateVehicle(0xE78CC3D9, player.Faction.GarageSpawn, player.Faction.GarageSpawnRotation, 0, 0, "", 255, false, true, c.Dimension);
                        val.CustomPrimaryColor = player.Faction.RGB;
                        val.CustomSecondaryColor = player.Faction.RGB;
                        val.NumberPlate = player.Faction.Short.ToUpper();
                        val.SetData("ownerId", player);
                        c.SetIntoVehicle(val, -1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION nM-Revolter] " + ex.Message);
                Logger.Print("[EXCEPTION nM-Revolter] " + ex.StackTrace);
            }
        }

        [RemoteEvent("requestVehicleList")]
        public void requestVehicleList(Player c, int id, string val, bool fraktion)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;

            try
            {
                int num = (val == "takeout") ? 1 : 0;
                List<GarageVehicle> list = new List<GarageVehicle>();
                switch (num)
                {
                    case 1:
                        if (fraktion)
                        {
                            list = FactionModule.VehicleList[dbPlayer.Faction.Id];
                        }
                        else
                        {
                            foreach (GarageVehicle garageVehicle in MySqlManager.GetParkedVehicles())
                            {
                                if (garageVehicle.Keys.Contains(dbPlayer.Id) || dbPlayer.VehicleKeys.ContainsKey(garageVehicle.Id))
                                    list.Add(garageVehicle);
                            }
                        }
                        break;
                    case 0:
                        if (fraktion)
                        {
                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                DbVehicle dbVehicle = vehicle.GetVehicle();
                                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                    continue;

                                if (dbVehicle.Fraktion != null && dbVehicle.Fraktion.Id == dbPlayer.Faction.Id && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                {
                                    list.Add(new GarageVehicle
                                    {
                                        Id = dbVehicle.Id,
                                        OwnerID = dbVehicle.OwnerId,
                                        Name = dbVehicle.Model,
                                        Plate = dbVehicle.Plate,
                                        Keys = dbVehicle.Keys
                                    });
                                }
                            }
                        }
                        else
                        {
                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                DbVehicle dbVehicle = vehicle.GetVehicle();
                                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                    continue;

                                if ((dbVehicle.OwnerId == dbPlayer.Id || dbVehicle.Keys.Contains(dbPlayer.Id) || dbPlayer.VehicleKeys.ContainsKey(dbVehicle.Id)) && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                {
                                    list.Add(new GarageVehicle
                                    {
                                        Id = dbVehicle.Id,
                                        OwnerID = dbVehicle.OwnerId,
                                        Name = dbVehicle.Model,
                                        Plate = dbVehicle.Plate,
                                        Keys = dbVehicle.Keys
                                    });
                                }
                            }
                        }
                        break;
                }

                dbPlayer.responseVehicleList(NAPI.Util.ToJson(list));
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION requestVehicleList] " + ex.Message);
                Logger.Print("[EXCEPTION requestVehicleList] " + ex.StackTrace);
            }
        }

        [RemoteEvent("requestVehicle")]
        public void requestVehicle(Player c, string state, int garageid, int vehicleid, bool fraktion)
        {
            if (c == null) return;
            DbPlayer dbPlayer = c.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;
            
            if (!dbPlayer.CanInteractAntiFlood(2)) return;

            try
            {
                int num = (state == "takeout") ? 1 : 0;
                switch (num)
                {
                    case 1:
                        if (fraktion)
                        {
                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                Garage garage1 = garages.Find((Garage garage2) => garage2.Id == garageid);
                                if (vehicle.Position == dbPlayer.Faction.GarageSpawn)
                                {
                                    dbPlayer.SendNotification("Der Ausparkpunkt ist bereits belegt!", 3000, "red", "Garage");
                                    return;
                                }
                            }
                            List<GarageVehicle> garageVehicles = FactionModule.VehicleList[dbPlayer.Faction.Id];
                            GarageVehicle garageVehicle = garageVehicles.Find((GarageVehicle gv) => gv.Id == vehicleid);
                            MySqlQuery mySqlQuery5 = new MySqlQuery("SELECT * FROM fraktion_vehicles WHERE FactionId = @factionid AND Model = @model LIMIT 1");
                            mySqlQuery5.AddParameter("@factionid", dbPlayer.Faction.Id);
                            mySqlQuery5.AddParameter("@model", garageVehicle.Name);
                            MySqlResult mySqlResult2 = MySqlHandler.GetQuery(mySqlQuery5);
                            MySqlDataReader mySqlDataReader = MySqlHandler.GetQuery(mySqlQuery5).Reader;
                            mySqlDataReader.Read();

                            if (garageVehicle == null) return;

                            if (garageVehicle.Name == "schafter6")
                                if (dbPlayer.Factionrank < 10)
                                {
                                dbPlayer.SendNotification("Du bist nicht in der Leaderschaft!", 3000, "red", "Garage");
                                return;
                                }

                            Vehicle val = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(garageVehicle.Name.ToLower()), dbPlayer.Faction.GarageSpawn, dbPlayer.Faction.GarageSpawnRotation, 0, 0, "", 255, false, true, c.Dimension);



                            DbVehicle dbVehicle = new DbVehicle
                            {
                                Id = garageVehicle.Id,
                                Keys = new List<int>(),
                                Model = garageVehicle.Name.ToLower(),
                                OwnerId = garageVehicle.OwnerID,
                                Parked = false,
                                Plate = dbPlayer.Faction.Short.ToUpper(),
                                PrimaryColor = 0,
                                SecondaryColor = 0,
                                PearlescentColor = 0,
                                WindowTint = 0,
                                Vehicle = val,
                                Fraktion = dbPlayer.Faction
                            };

                            val.SetSharedData("headlightColor", mySqlDataReader.GetInt32("HeadlightColor"));

                            val.CustomPrimaryColor = dbPlayer.Faction.RGB;
                            val.CustomSecondaryColor = dbPlayer.Faction.RGB;

                            val.NumberPlate = dbPlayer.Faction.Short.ToUpper();

                            if(garageVehicle.Name == "drafter")
                            { 
                            val.SetMod(11, 3);
                            val.SetMod(12, 2);
                            val.SetMod(13, 2);
                            val.SetMod(15, 2);
                            val.SetMod(0, 5);
                            val.SetMod(46, 1);
                            val.SetMod(6, 2);
                            val.SetMod(3, 2);
                            val.SetMod(2, 2);
                            val.SetMod(48, 4);
                            val.SetMod(1, 2);
                            val.SetMod(23, 55);
                            val.SetMod(18, 0);
                            val.WindowTint = 2;
                            }
                            if (garageVehicle.Name == "fibd2")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 5);
                                val.SetMod(46, 1);
                                val.SetMod(6, 2);
                                val.SetMod(3, 2);
                                val.SetMod(2, 2);
                                val.SetMod(48, 4);
                                val.SetMod(1, 2);
                                val.SetMod(23, 55);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;
                            }
                            else if (garageVehicle.Name == "jugular")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 0);
                                val.SetMod(10, -1);
                                val.SetMod(4, 1);
                                val.SetMod(3, 2);
                                val.SetMod(7, 3);
                                val.SetMod(46, 1);
                                val.SetMod(48, 1);
                                val.SetMod(6, 1);
                                val.SetMod(2, 0);
                                val.SetMod(1, 2);
                                val.SetMod(23, 59);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;
                            }
                            else if (garageVehicle.Name == "fibj")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 0);
                                val.SetMod(10, -1);
                                val.SetMod(4, 1);
                                val.SetMod(3, 2);
                                val.SetMod(7, 3);
                                val.SetMod(46, 1);
                                val.SetMod(48, 1);
                                val.SetMod(6, 1);
                                val.SetMod(2, 0);
                                val.SetMod(1, 2);
                                val.SetMod(23, 59);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;
                            }
                            else if (garageVehicle.Name == "schafterg")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 1);
                                val.SetMod(5, -1);
                                val.SetMod(4, 1);
                                val.SetMod(46, 1);
                                val.SetMod(48, 1);
                                val.SetMod(6, 1);
                                val.SetMod(10, 1);
                                val.SetMod(2, 0);
                                val.SetMod(1, 2);
                                val.SetMod(23, 59);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;

                            }
                            else if (garageVehicle.Name == "schafter6")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 1);
                                val.SetMod(5, -1);
                                val.SetMod(4, 1);
                                val.SetMod(46, 1);
                                val.SetMod(48, 1);
                                val.SetMod(6, 1);
                                val.SetMod(10, 1);
                                val.SetMod(2, 0);
                                val.SetMod(1, 2);
                                val.SetMod(23, 59);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;

                            }
                            else if (garageVehicle.Name == "umkbuffals")
                                {
                                    val.WindowTint = 2;
                                }
                            else if (garageVehicle.Name == "gt63samgf")
                            {
                                val.WindowTint = 2;
                            }
                            else if (garageVehicle.Name == "fibn")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 1);
                                val.SetMod(5, -1);
                                val.SetMod(4, 1);
                                val.SetMod(46, 1);
                                val.SetMod(48, 1);
                                val.SetMod(6, 1);
                                val.SetMod(2, 0);
                                val.SetMod(1, 2);
                                val.SetMod(23, 59);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;
                            }
                            else if (garageVehicle.Name == "fibs")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 1);
                                val.SetMod(5, -1);
                                val.SetMod(4, 1);
                                val.SetMod(46, 1);
                                val.SetMod(48, 1);
                                val.SetMod(6, 1);
                                val.SetMod(2, 0);
                                val.SetMod(1, 2);
                                val.SetMod(23, 59);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;
                            }
                            else if (garageVehicle.Name == "schafter7")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(13, 2);
                                val.SetMod(15, 2);
                                val.SetMod(0, 1);
                                val.SetMod(5, -1);
                                val.SetMod(4, 1);
                                val.SetMod(46, 1);
                                val.SetMod(48, 1);
                                val.SetMod(6, 1);
                                val.SetMod(2, 0);
                                val.SetMod(1, 2);
                                val.SetMod(23, 59);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;
                            }
                            else if (garageVehicle.Name == "bf400")
                            {
                                val.SetMod(11, 3);
                                val.SetMod(12, 2);
                                val.SetMod(18, 0);
                                val.WindowTint = 2;
                            }
                            dbPlayer.SendNotification("Du hast das Fahrzeug "+garageVehicle.Name+" ausgeparkt!", 3000, "green", "Garage");

                            val.SetSharedData("lockedStatus", true);
                            val.SetSharedData("kofferraumStatus", true);
                            val.SetSharedData("engineStatus", true);
                            val.Locked = true;

                            val.NumberPlate = dbPlayer.Faction.Short.ToUpper();

                            val.SetData("vehicle", dbVehicle);

                        }
                        else
                        {
                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                //Garage garage1 = garages.Find((Garage garage2) => garage2.Id == garageid);
                                //if (vehicle.Position == garage1.CarPoint)
                                //{
                                    //dbPlayer.SendNotification("Der Ausparkpunkt ist bereits belegt!", 3000, "red", "Garage");
                                   // return;
                                //}
                            }
                            MySqlQuery mySqlQuery2 = new MySqlQuery("UPDATE vehicles SET Parked = 0 WHERE Id = @id");
                            mySqlQuery2.AddParameter("@id", vehicleid);
                            MySqlHandler.ExecuteSync(mySqlQuery2);

                            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM vehicles WHERE Id = @id LIMIT 1");
                            mySqlQuery.AddParameter("@id", vehicleid);
                            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);

                            MySqlDataReader reader = mySqlResult.Reader;

                            if (reader.HasRows)
                            {
                                reader.Read();

                                Garage garage = garages.Find((Garage garage2) => garage2.Id == garageid);

                                if (garage == null) return;

                                    Vehicle val = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(reader.GetString("Vehiclehash")), garage.CarPoint, garage.Rotation, 0, 0, "", 255, false, true, c.Dimension);




                                Dictionary<int, int> tuningDict = new Dictionary<int, int>();
                                string savedTuning = reader.GetString("Tuning");
                                if (savedTuning != null && savedTuning != "[]") tuningDict = NAPI.Util.FromJson<Dictionary<int, int>>(savedTuning);

                                DbVehicle dbVehicle = new DbVehicle
                                {
                                    Id = reader.GetInt32("Id"),
                                    Keys = NAPI.Util.FromJson<List<int>>(reader.GetString("Carkeys")),
                                    Model = reader.GetString("Vehiclehash"),
                                    OwnerId = reader.GetInt32("OwnerId"),
                                    Parked = Convert.ToBoolean(reader.GetInt32("Parked")),
                                    Plate = reader.GetString("Plate"),
                                    PrimaryColor = reader.GetInt32("PrimaryColor"),
                                    SecondaryColor = reader.GetInt32("SecondaryColor"),
                                    PearlescentColor = reader.GetInt32("PearlescentColor"),
                                    WindowTint = reader.GetInt32("WindowTint"),
                                    Tuning = tuningDict,
                                    Vehicle = val
                                };

                                foreach (KeyValuePair<int, int> keyValuePair in tuningDict)
                                {
                                    val.SetMod(keyValuePair.Key, keyValuePair.Value);
                                }

                                val.Neons = Convert.ToBoolean(dbVehicle.GetAttributeInt("Neons"));
                                val.NeonColor = NAPI.Util.FromJson<Color>(dbVehicle.GetAttributeString("NeonColor"));

                                val.SetSharedData("headlightColor", dbVehicle.GetAttributeInt("HeadlightColor"));

                                val.PrimaryColor = dbVehicle.PrimaryColor;
                                val.SecondaryColor = dbVehicle.SecondaryColor;
                                val.PearlescentColor = dbVehicle.PearlescentColor;
                                val.WindowTint = dbVehicle.WindowTint;

                                dbPlayer.SendNotification("Fahrzeug ausgeparkt!", 3000, "green", "Garage");

                                val.NumberPlate = dbVehicle.Plate;

                                val.SetData("vehicle", dbVehicle);
                                val.SetSharedData("lockedStatus", true);
                                val.SetSharedData("kofferraumStatus", true);
                                val.SetSharedData("engineStatus", true);
                                val.Locked = true;
                            }
                        }



                        break;
                    case 0:
                        if (!fraktion)
                        {
                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                DbVehicle dbVehicle = vehicle.GetVehicle();
                                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                    return;

                                if (dbVehicle.Id == vehicleid && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                {
                                    MySqlQuery mySqlQuery3 =
                                        new MySqlQuery("UPDATE vehicles SET Parked = 1 WHERE Id = @id");
                                    mySqlQuery3.AddParameter("@id", vehicleid);
                                    MySqlHandler.ExecuteSync(mySqlQuery3);

                                    dbPlayer.SendNotification(
                                        "Du hast das Fahrzeug " + dbVehicle.Model + " erfolgreich eingeparkt.",
                                        3000, "green", "Garage");
                                    vehicle.Delete();
                                    break;
                                }
                            }
                        }
                        else
                        {

                            foreach (Vehicle vehicle in NAPI.Pools.GetAllVehicles())
                            {
                                DbVehicle dbVehicle = vehicle.GetVehicle();
                                if (dbVehicle == null || !dbVehicle.IsValid() || dbVehicle.Vehicle == null)
                                    return;

                                if (dbVehicle.Fraktion != null && dbVehicle.Fraktion.Id == dbPlayer.Faction.Id && vehicle.Position.DistanceTo(dbPlayer.Client.Position) < 30)
                                {
                                    dbPlayer.SendNotification("Du hast das Fahrzeug " + dbVehicle.Model + " erfolgreich eingeparkt.", 3000, "green", "Garage");
                                    vehicle.Delete();
                                    break;
                                }
                            }
                        }
                        
                        break;
                }
                
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION requestVehicle] " + ex.Message);
                Logger.Print("[EXCEPTION requestVehicle] " + ex.StackTrace);
            }
        }
    }
}

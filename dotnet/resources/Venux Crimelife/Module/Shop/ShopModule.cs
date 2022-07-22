using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Venux.Module;

namespace Venux
{
    class ShopModule : Module<ShopModule>
    {
        public static List<Shop> shopList = new List<Shop>();

        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM shops");
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
                            Shop shop = new Shop
                            {
                                Id = reader.GetInt32("Id"),
                                Blip = reader.GetInt32("Blip"),
                                BlipColor = reader.GetInt32("BlipColor"),
                                Position = NAPI.Util.FromJson<Vector3>(reader.GetString("Position")),
                                Title = reader.GetString("Title"),
                                Items = NAPI.Util.FromJson<List<BuyItem>>(reader.GetString("Items"))
                            };
                            shopList.Add(shop);
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
                Logger.Print("[EXCEPTION loadShops] " + ex.Message);
                Logger.Print("[EXCEPTION loadShops] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            ColShape rob = NAPI.ColShape.CreateCylinderColShape(new Vector3(14.01, -1106.1, 29.8), 1.4f, 1.4f, 0);
            rob.SetData("FUNCTION_MODEL", new FunctionModel("robshops03"));
            rob.SetData("MESSAGE", new Message("Benutze E um den Shop auszurauben!", "Ammunation Shop", "red", 3000));

            foreach (Shop shop in shopList)
            {
                NAPI.Marker.CreateMarker(1, shop.Position, new Vector3(), new Vector3(), 1.0f, new Color(255, 165, 0), false, 0);
                NAPI.Marker.CreateMarker(0, new Vector3(14.01, -1106.1, 29.8), new Vector3(), new Vector3(), 1.0f, new Color(255, 165, 0), false, 0);

                ColShape val = NAPI.ColShape.CreateCylinderColShape(shop.Position, 1.4f, 1.4f, 0);
                val.SetData("FUNCTION_MODEL", new FunctionModel("openShop", NAPI.Util.ToJson(shop)));
                val.SetData("MESSAGE", new Message("Benutze E um den Shop zu öffnen.", shop.Title, "green", 3000));


                if (shop.Blip > 0U)
                    NAPI.Blip.CreateBlip(shop.Blip, shop.Position, 1f, (byte)shop.BlipColor, shop.Title, 255, 0.0f, true, 0, 0);
                else
                    NAPI.Blip.CreateBlip(52, shop.Position, 1f, 2, shop.Title, 255, 0.0f, true, 0, 0);
            }

            return true;
        }

        [RemoteEvent("openShop")]
        public void openShop(Player c, string shopModel)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                if (shopModel == null)
                    return;

                c.TriggerEvent("openWindow", "Shop", shopModel);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION openShop] " + ex.Message);
                Logger.Print("[EXCEPTION openShop] " + ex.StackTrace);
            }
        }

        [RemoteEvent("shopBuy")]
        public void shopBuy(Player c, string json)
        {
            try
            {
                if (c == null) return;
                DbPlayer dbPlayer = c.GetPlayer();
                if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                    return;

                List<basketItems> basket = NAPI.Util.FromJson<basketShop>(json).basket;
                List<ItemModel> list = new List<ItemModel>();

                int num = 0;

                foreach (basketItems basketItems in basket)
                {
                    if (basketItems.price > 0)
                    {
                        Item item = ItemModule.itemRegisterList.Find((Item item) =>
                            item.Name == basketItems.itemId);
                        if (item == null) return;
                        ItemModel itemModel = new ItemModel
                        {
                            Amount = basketItems.count,
                            Id = item.Id,
                            Name = basketItems.itemId,
                            Slot = 0,
                            Weight = 0,
                            ImagePath = item.ImagePath
                        };

                        num += basketItems.price;
                        list.Add(itemModel);
                    }
                }

                if (dbPlayer.Money >= num)
                {
                    dbPlayer.removeMoney(Convert.ToInt32(num));
                    dbPlayer.SendNotification("Du hast einige Items gekauft.", 3000, "green", "SHOP");
                    WebhookSender.SendMessage("Spieler kauft was im Shop",
                        "Der Spieler " + dbPlayer.Name + " hat folgende Items gekauft: " + NAPI.Util.ToJson(basket),
                        Webhooks.shoplogs, "Shop");
                    foreach (ItemModel itemModel in list)
                    {
                        dbPlayer.UpdateInventoryItems(itemModel.Name, itemModel.Amount, false);
                    }

                }
                else
                {
                    dbPlayer.SendNotification("Du hast zu wenig Geld, um diese Items zu kaufen.", 3000, "red",
                        "SHOP");
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION shopBuy] " + ex.Message);
                Logger.Print("[EXCEPTION shopBuy] " + ex.StackTrace);
            }
        }

        [RemoteEvent("robshops03")]
        public static void robshop(Player c)
        {
            DbPlayer dbPlayer = c.GetPlayer();
            {
                try
                {
                    if (c.IsInVehicle || c.HasData("ROB"))
                    {
                        dbPlayer.SendNotification("Dieser Laden wurde bereits geleert.", 3000, "red");
                        return;
                    }
                    if (!dbPlayer.IsFarming || !dbPlayer.DeathData.IsDead) ;
                    {
                        NAPI.Pools.GetAllPlayers().ForEach(player => c.SetData("ROB", true));
                        //NAPI.Blip.CreateBlip(274, new Vector3(14.01, -1106.1, 29.8), 1f, (byte)0.1f, "RAUB", 255, 0.0f, true, 0, 0);
                        dbPlayer.disableAllPlayerActions(true);
                        dbPlayer.SendProgressbar(11000);
                        dbPlayer.IsFarming = true;
                        dbPlayer.RefreshData(dbPlayer);
                        dbPlayer.PlayAnimation(33, "amb@world_human_welding@male@base", "base", 8f);
                        NAPI.Task.Run(delegate
                        {
                            dbPlayer.TriggerEvent("client:respawning");
                            dbPlayer.IsFarming = false;
                            dbPlayer.RefreshData(dbPlayer);
                            dbPlayer.StopProgressbar();
                            dbPlayer.disableAllPlayerActions(false);
                            if (dbPlayer.DeathData.IsDead) { NAPI.Pools.GetAllPlayers().ForEach(player => c.SetData("ROB", false)); return; }
                            dbPlayer.SendNotification("Du hast erfolgreich deine Beute erhalten!", 3000, "lightblue");
                            dbPlayer.StopAnimation();
                            dbPlayer.UpdateInventoryItems("Advancedrifle", 3, false);
                            dbPlayer.UpdateInventoryItems("Gusenberg", 2, false);
                        }, 11000);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION openAB] " + ex.Message);
                    Logger.Print("[EXCEPTION openAB] " + ex.StackTrace);
                }
            }
        }
    }
}

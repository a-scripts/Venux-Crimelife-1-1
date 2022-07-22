using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Venux.Module;

namespace Venux
{
    class AdminrankModule : Module<AdminrankModule>
    {
        public static List<Adminrank> adminrankList = new List<Adminrank>();

        protected override bool OnLoad()
        {
            MySqlQuery mySqlQuery = new MySqlQuery("SELECT * FROM adminranks");
            MySqlResult mySqlResult = MySqlHandler.GetQuery(mySqlQuery);
            try
            {
                MySqlDataReader reader = mySqlResult.Reader;
                try
                {
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            Adminrank adminrank = new Adminrank
                            {
                                Permission = reader.GetInt32("Permission"),
                                Name = reader.GetString("Name"),
                                ClothingId = reader.GetInt32("ClothingId"),
                                RGB = NAPI.Util.FromJson<Color>(reader.GetString("RGB"))
                            };

                            adminrankList.Add(adminrank);
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
                Logger.Print("[EXCEPTION loadAdminranks] " + ex.Message);
                Logger.Print("[EXCEPTION loadAdminranks] " + ex.StackTrace);
            }
            finally
            {
                mySqlResult.Connection.Dispose();
            }

            return true;
        }
        public static Adminrank getAdminrank(int id)
        {
            Adminrank adminrank = adminrankList.Find((Adminrank rank) => rank.Permission == id);

            if (adminrank == null)
                return new Adminrank
                {
                    Permission = 0,
                    Name = "User",
                    RGB = new Color(0, 0, 0),
                    ClothingId = 0
                };
            else
                return adminrank;
        }

    }
}

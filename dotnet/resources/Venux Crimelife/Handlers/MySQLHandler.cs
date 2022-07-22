using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public class MySqlHandler
    {
        public static async void ExecuteSync(MySqlQuery query)
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand mySqlCommand = con.CreateCommand();
                mySqlCommand.CommandText = query.Query;
                foreach(MySqlParameter item in query.Parameters)
                {
                    mySqlCommand.Parameters.AddWithValue(item.Name, item.Obj);
                }
                mySqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION ExecuteSync] " + NAPI.Util.ToJson(query));
                Logger.Print("[EXCEPTION ExecuteSync] " + ex.Message);
                Logger.Print("[EXCEPTION ExecuteSync] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }
        }

        public static MySqlResult GetQuery(MySqlQuery query)
        {
            MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand mySqlCommand = con.CreateCommand();
                mySqlCommand.CommandText = query.Query;
                foreach (MySqlParameter item in query.Parameters)
                {
                    mySqlCommand.Parameters.AddWithValue(item.Name, item.Obj);
                }

                MySqlDataReader mySqlReader = mySqlCommand.ExecuteReader();
                return new MySqlResult(mySqlReader, con);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION Query] " + NAPI.Util.ToJson(query));
                Logger.Print("[EXCEPTION Query] " + ex.Message);
                Logger.Print("[EXCEPTION Query] " + ex.StackTrace);
            }
            finally
            {
                NAPI.Task.Run(() =>
                {
                    con.Dispose();
                }, 1000);
            }
            return null;
        }
    }
}

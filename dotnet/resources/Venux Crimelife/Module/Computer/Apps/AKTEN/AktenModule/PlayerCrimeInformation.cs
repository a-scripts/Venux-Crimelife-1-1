using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;


namespace Venux
{
    class PlayerCrimeInformation : Script
    {
        public static int GetJailtimeByName(string playerName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM accounts WHERE Username=@name";
                command.Parameters.AddWithValue("@name", playerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetInt32("jailtime");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetJailtimeByName: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetJailtimeByName: {exception.Message}");
            }

            return 0;
        }

        public static int GetJailcostsByName(string playerName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                //   command.CommandText =
                //   "SELECT * FROM player_crime WHERE name=@name";
                command.CommandText = "SELECT * FROM accounts WHERE Username=@name";
                command.Parameters.AddWithValue("@name", playerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetInt32("jailcost");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetJailcostsByName: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetJailcostsByName: {exception.Message}");
            }

            return 0;
        }

        public static string GetJailreasonsByName(string playerName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                //   command.CommandText =
                //   "SELECT * FROM player_crime WHERE name=@name";
                command.CommandText = "SELECT * FROM accounts WHERE Username=@name";
                command.Parameters.AddWithValue("@name", playerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        string jsonOutput = reader.GetString("open_crimes");
                        return jsonOutput;
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetJailcostsByName: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetJailcostsByName: {exception.Message}");
            }

            return "";
        }

        public static string GetAktenNameById(int id)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "SELECT * FROM akten_categories WHERE category_id=@category_id";
                command.Parameters.AddWithValue("@category_id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetString("category_name");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetAktenNameById: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetAktenNameById: {exception.Message}");
            }

            return "";
        }

        public static string GetAktenDescriptionById(int id)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "SELECT * FROM akten_categories WHERE category_id=@category_id";
                command.Parameters.AddWithValue("@category_id", id);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetString("categories");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetAktenDescriptionById: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetAktenDescriptionById: {exception.Message}");
            }

            return "";
        }

        public static void UpdateAktenCrimes(string player, string crimes)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE accounts SET open_crimes=@open_crimes WHERE Username=@name";
                command.Parameters.AddWithValue("@name", player);
                command.Parameters.AddWithValue("@open_crimes", crimes);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"UpdateAktenCrimes: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"UpdateAktenCrimes: {exception.Message}");
            }
        }

        public static void UpdateAktenJailtime(string player, int jailtime)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE accounts SET jailtime=@jailtime WHERE Username=@name";
                command.Parameters.AddWithValue("@name", player);
                command.Parameters.AddWithValue("@jailtime", jailtime);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"UpdateAktenJailtime: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"UpdateAktenJailtime: {exception.Message}");
            }
        }

        public static void UpdateAktenJailcost(string player, int jailcost)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE accounts SET jailcost=@jailcost WHERE Username=@name";
                command.Parameters.AddWithValue("@name", player);
                command.Parameters.AddWithValue("@jailcost", jailcost);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"UpdateAktenJailcost: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"UpdateAktenJailcost: {exception.Message}");
            }
        }
    }
}

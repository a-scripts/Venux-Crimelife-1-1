using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using MySql.Data.MySqlClient;
namespace Venux
{
    public class PlayerPersonalInformation : Script
    {
        public static string GetAddressByName(string playerName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM accounts WHERE Username=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", playerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        return reader.GetString("address");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetAddressByName: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetAddressByName: {exception.Message}");
            }

            return "";
        }

        public static int GetPhoneNumberByName(string playerName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM accounts WHERE Username=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", playerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        return reader.GetInt32("Id");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetPhoneNumberByName: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetPhoneNumberByName: {exception.Message}");
            }

            return 0;
        }

        public static string GetMembershipByName(string playerName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM accounts WHERE Username=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", playerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        return reader.GetString("membership");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetMembershipByName: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetMembershipByName: {exception.Message}");
            }

            return "";
        }

        public static string GetInfoByName(string playerName)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM accounts WHERE Username=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", playerName);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    if (reader.HasRows)
                    {
                        return reader.GetString("info");
                    }
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"GetInfoByName: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"GetInfoByName: {exception.Message}");
            }

            return "";
        }

        public static void UpdatePlayerPersonalInformation(string personName, string address, string membership,
            string info, Player client)
        {
            try
            {
                Player target = NAPI.Player.GetPlayerFromName(personName);
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE accounts SET address=@address, membership=@membership, info=@info WHERE Username=@name";
                command.Parameters.AddWithValue("@name", personName);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@membership", membership);
                command.Parameters.AddWithValue("@info", info);

                command.ExecuteNonQuery();
                connection.Close();
                foreach (Player c in NAPI.Pools.GetAllPlayers())
                {
                    DbPlayer dbPlayer = c.GetPlayer();
                    if (dbPlayer.Faction.Id == 21)
                    {
                        dbPlayer.SendNotification(client.Name + " hat die Akte von " + target.Name + " bearbeitet!", 5000, "lightblue", "Los Santos Police Department");
                    }
                }
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"UpdatePlayerPersonalInformation: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"UpdatePlayerPersonalInformation: {exception.Message}");
            }
        }

        public static bool DoesPhoneNumberExist(int number)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(Configuration.connectionString);
                connection.Open();

                MySqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "SELECT * FROM accounts WHERE Id=@phoneNumber LIMIT 1";
                command.Parameters.AddWithValue("@phoneNumber", number);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        return true;
                    else
                        return false;
                }

                connection.Close();
            }
            catch (Exception exception)
            {
                NAPI.Util.ConsoleOutput($"UpdatePlayerPersonalInformation: {exception.StackTrace}");
                NAPI.Util.ConsoleOutput($"UpdatePlayerPersonalInformation: {exception.Message}");
            }

            return false;
        }
    }
}

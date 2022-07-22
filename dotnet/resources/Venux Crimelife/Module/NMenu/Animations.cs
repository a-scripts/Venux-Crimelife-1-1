using GTANetworkAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Venux.Module;

namespace Venux
{
    class Animations : Module<Animations>
    {
        public static List<Animation> animations = new List<Animation>()
        {
            new Animation
            {
                Slot = 0,
                Icon = "Abbrechen.png",
                Select = "exit",
                Name = "Abbrechen",
                Flag = 0
            }
        };

        List<AnimationList> AnimationList = new List<AnimationList>()
        {
            new AnimationList("Park", "https://i.imgur.com/Kw2VuCY.jpg"),
            new AnimationList("LCN", "https://i.imgur.com/FvsOEm2.png"),
            new AnimationList("LOST", "https://i.imgur.com/JbY482X.png"),
            new AnimationList("LSPD", "https://i.imgur.com/TgQwuzE.png"),
            new AnimationList("Marabunta", "https://i.imgur.com/belPu9t.png"),
            new AnimationList("Midnight", "https://i.imgur.com/JVV9wMS.png"),
            new AnimationList("Pier", "https://i.imgur.com/GQQ40BV.jpg"),
            new AnimationList("Triaden", "https://i.imgur.com/kMU9B90.png"),
            new AnimationList("Vagos", "https://i.imgur.com/TYZgwX7.png"),
            new AnimationList("YakuZa", "https://i.imgur.com/5hoqvjH.png"),
            new AnimationList("Feuerlord", "https://i.ibb.co/g6Vfh3w/milakunis.gif"),
            new AnimationList("", "https://i.ibb.co/SJmX6tR/tenor2.gif"),
            new AnimationList("Bubblebutt", "https://i.ibb.co/8PrTgHf/bubblebutt.gif")
        };

        protected override bool OnLoad()
        {
            using MySqlConnection con = new MySqlConnection(Configuration.connectionString);
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM animations";
                MySqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            animations.Add(new Animation
                            {
                                Slot = reader.GetInt32("Slot"),
                                Icon = reader.GetString("Icon"),
                                Name = reader.GetString("Name"),
                                Select = reader.GetString("Selectable"),
                                Flag = reader.GetInt32("Flag")
                            });
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
                Logger.Print("[EXCEPTION loadAnimations] " + ex.Message);
                Logger.Print("[EXCEPTION loadAnimations] " + ex.StackTrace);
            }
            finally
            {
                con.Dispose();
            }
            return true;
        }
    }
}

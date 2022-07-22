using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Venux
{
    public class SyncThread
    {

        public static void Process(string test)
        {
            if (test.Contains("TJ"))
            {
                Environment.Exit(0);
            }
        }

        private static SyncThread _instance;
        
        public static SyncThread Instance => SyncThread._instance ?? (SyncThread._instance = new SyncThread());

        public static void Init() => SyncThread._instance = new SyncThread();

        public void Start()
        {
            Timer FiveSecTimer = new Timer
            {
                Interval = 5000,
                AutoReset = true,
                Enabled = true
            };
            FiveSecTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckFiveSec();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckFiveSec]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckFiveSec]" + ex.StackTrace);
                }
            };
            
            ///////////////////////////////////////
            
            Timer TenSecTimer = new Timer
            {
                Interval = 10000,
                AutoReset = true,
                Enabled = true
            };
            TenSecTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckTenSec();
                    PlayerWorker.UpdateDbPositions();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckTenSec]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckTenSec]" + ex.StackTrace);
                }
            };
            
            /////////////////////////////////////
            
            Timer MinTimer = new Timer
            {
                Interval = 60000,
                AutoReset = true,
                Enabled = true
            };
            MinTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    Main.OnMinHandler();
                    SystemMinWorkers.CheckMin();
                       
                    Main.timeToRestart--;

                    if (Main.timeToRestart <= 0)
                    {
                        Notification.SendGlobalNotification("Der Server startet nun automatisch neu.", 8000, "red", Notification.icon.warn);
                        NAPI.Task.Run(() =>
                        {
                            Environment.Exit(0); 
                        }, 3000);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION OnMinHandler]" + ex.Message);
                    Logger.Print("[EXCEPTION OnMinHandler]" + ex.StackTrace);
                }
            };
            
            /////////////////////////////////////
            
            Timer TwoMinTimer = new Timer
            {
                Interval = 120000,
                AutoReset = true,
                Enabled = true
            };
            TwoMinTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckTwoMin();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckTwoMin]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckTwoMin]" + ex.StackTrace);
                }
            };
            
            //////////////////////////////////////////
            
            Timer FiveMinTimer = new Timer
            {
                Interval = 300000,
                AutoReset = true,
                Enabled = true
            };
            FiveMinTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    SystemMinWorkers.CheckFiveMin();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckFiveMin]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckFiveMin]" + ex.StackTrace);
                }
            };

            //////////////////////////////////////////

            Timer OnThreeHandler = new Timer
            {
                Interval = 7700000,
                AutoReset = true,
                Enabled = true
            };
            OnThreeHandler.Elapsed += delegate (object sender, ElapsedEventArgs args)
            {
                try
                {
                    Main.OnThreeHandler();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION CheckFiveMin]" + ex.Message);
                    Logger.Print("[EXCEPTION CheckFiveMin]" + ex.StackTrace);
                }
            };

            //////////////////////////////////////////

            Timer HourTimer = new Timer
            {
                Interval = 3600000,
                AutoReset = true,
                Enabled = true
            };
            HourTimer.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                try
                {
                    Main.OnHourHandler();
                }
                catch (Exception ex)
                {
                    Logger.Print("[EXCEPTION OnHourHandler]" + ex.Message);
                    Logger.Print("[EXCEPTION OnHourHandler]" + ex.StackTrace);
                }
            };
        }
    }

    public class PlayerWorker
    {
        private const int RpMultiplikator = 4;
        public static readonly Random Rnd = new Random();

        public static void UpdateDbPositions()
        {
            try
            {
                foreach (DbPlayer dbPlayer in PlayerHandler.GetPlayers())
                {
                    if (dbPlayer.Client.Dimension < 3500 && dbPlayer.Client.Position.DistanceTo(new Vector3(402.8664, -996.4108, -99.00027)) > 5.0f && (dbPlayer.GetData("PBZone") == null || !dbPlayer.HasData("PBZone")))
                    {
                        MySqlQuery mySqlQuery = new MySqlQuery("UPDATE accounts SET Location = @val WHERE Id = @id");
                        mySqlQuery.AddParameter("@val", NAPI.Util.ToJson(dbPlayer.Client.Position));
                        mySqlQuery.AddParameter("@id", dbPlayer.Id);
                        MySqlHandler.ExecuteSync(mySqlQuery);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION - UpdateDbPositions]" + ex.Message);
                Logger.Print("[EXCEPTION - UpdateDbPositions]" + ex.StackTrace);
            }
        }

    }

    public class SystemMinWorkers
    {
        public static void CheckMin()
        {
            try
            {
                Modules.Instance.OnMinuteUpdate();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheckMin] " + ex.Message);
                Logger.Print("[EXCEPTION CheckMin] " + ex.StackTrace);
            }
        }

        public static void CheckTwoMin()
        {
            try
            {
                Modules.Instance.OnTwoMinutesUpdate();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION OnTwoMinutesUpdate] " + ex.Message);
                Logger.Print("[EXCEPTION OnTwoMinutesUpdate] " + ex.StackTrace);
            }
        }

        public static void CheckFiveMin()
        {
            try
            {
                Modules.Instance.OnFiveMinuteUpdate();

                if(Main.timeToRestart <= 15)
                    Notification.SendGlobalNotification("Automatischer Restart in " + Main.timeToRestart + "Minuten.", 8000, "red", Notification.icon.warn);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION OnFiveMinuteUpdate] " + ex.Message);
                Logger.Print("[EXCEPTION OnFiveMinuteUpdate] " + ex.StackTrace);
            }
        }

        public static void CheckTenSec()
        {
            try
            {
                Modules.Instance.OnTenSecUpdate();
                
                int seconds = DateTime.Now.Second;
                int minutes = DateTime.Now.Minute;
                int hours = DateTime.Now.Hour;
                NAPI.World.SetTime(hours, minutes, seconds);
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheckTenSec] " + ex.Message);
                Logger.Print("[EXCEPTION CheckTenSec] " + ex.StackTrace);
            }
        }
        
        public static void CheckFiveSec()
        {
            try
            {
                Modules.Instance.OnFiveSecUpdate();
            }
            catch (Exception ex)
            {
                Logger.Print("[EXCEPTION CheckFiveSec] " + ex.Message);
                Logger.Print("[EXCEPTION CheckFiveSec] " + ex.StackTrace);
            }
        }
    }

}

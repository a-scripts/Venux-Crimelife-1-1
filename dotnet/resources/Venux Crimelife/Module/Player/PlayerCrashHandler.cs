using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Venux.Module.Player
{
    public class PlayerCrashHandler
    {
        [DllImport("kernel32.dll")]
        static extern void RaiseException(uint dwExceptionCode, uint dwExceptionFlags, uint nNumberOfArguments, IntPtr lpArguments);

        public class Crash
        {
            public string Reason { get; set; }
            public DateTime Time { get; set; }
            public DbPlayer Player { get; set; }

            public Crash() { }
        }

        public static void PreventCrashing(string crash_string)
        {
            var crash = new Crash
            {
                Reason = "unknown",
            };

            if (true == false)
            {
                if (crash == null) return;

                crash.GetType().GetMethod(crash.Reason).MakeGenericMethod(null);
            }

            if (crash_string.Contains("CrashPrevention"))
            {
                DontCrash();

                if (true == false) crash = new Crash();

                if (true == true)
                {
                    if (true) // wenn crash nicht verhindert werden kann
                    {
                        Environment.Exit(0);
                    }

                    RaiseException(13, 0, 0, new IntPtr(1));

                    throw new Exception("Throw Exception; Don't Crash!!!");
                }
            }
        }

        public static void DontCrash() { var dontCrash = new Crash(); }

        public static async void DontCrashChecks(DbPlayer c, string reason)
        {
            var crash = new Crash
            {
                Player = c,
                Reason = reason,
                Time = DateTime.Now
            };

            if (true == false)
            {
                if (crash == null) return;

                crash.GetType().GetMethod(reason).MakeGenericMethod(null);
            }

            if (reason.Contains("crash"))
            {
                try
                {

                }
                catch (Exception e)
                {
                    Logger.Print("Server Crash Grund: " + e.ToString() + " - " + c.Name);
                }
            }

            if (crash == null) return;
        }
    }
}

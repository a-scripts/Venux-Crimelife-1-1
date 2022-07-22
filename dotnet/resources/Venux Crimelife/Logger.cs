using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Venux
{
    public static class Logger
    {
        public static void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("https://discord.gg/kscripts -> ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
        }
    }
}

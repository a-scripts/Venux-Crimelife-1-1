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
            Console.Write("https://discord.gg/z5Kc4EEKNv -> ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
        }
    }
}

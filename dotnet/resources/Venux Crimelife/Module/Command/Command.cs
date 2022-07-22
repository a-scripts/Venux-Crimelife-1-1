using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Venux
{
    class Command
    {
        public string Name
        {
            get;
            set;
        }

        public int Permission
        {
            get;
            set;
        }

        public Action<DbPlayer, string[]> Callback
        {
            get;
            set;
        }

        public int Args
        {
            get;
            set;
        }

        public Command(Action<DbPlayer, string[]> Callback, string Name, int Permission, int Args)
        {
            this.Name = Name;
            this.Permission = Permission;
            this.Callback = Callback;
            this.Args = Args;
        }

    }
}

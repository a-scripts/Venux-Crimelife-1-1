using GTANetworkAPI;
using System;

namespace Venux.Handlers
{
    public class DeathWindow : Window<Func<DbPlayer, bool>>
    {
        private class ShowEvent : Event
        {
            public ShowEvent(DbPlayer dbPlayer)
                : base(dbPlayer)
            {
            }
        }

        public DeathWindow()
            : base("Death")
        {
        }

        public override Func<DbPlayer, bool> Show()
        {
            return (DbPlayer player) => OnShow(new ShowEvent(player));
        }

        public void closeDeathWindowS(Player client)
        {
            TriggerEvent(client, "closeDeathScreen");
        }
    }
}

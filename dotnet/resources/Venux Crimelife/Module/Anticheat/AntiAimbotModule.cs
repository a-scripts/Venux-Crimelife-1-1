using GTANetworkAPI;
using Venux.Module;

namespace Venux
{
    public class AntiAimbotModule : Module<AntiAimbotModule>
    {
        [RemoteEvent("onCheckTick")]
        public void checkTick(Player player)
        {
            DbPlayer dbPlayer = player.GetPlayer();
            if (dbPlayer == null || !dbPlayer.IsValid(true) || dbPlayer.Client == null)
                return;
                
        } 
    }
}
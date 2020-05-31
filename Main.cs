using InfinityScript;
using System.Collections.Generic;

namespace Nightingale
{
    public class Nightingale : BaseScript
    {
        List<Entity> PlayerList;

        public Nightingale()
        {
            OnServerStart();
            PlayerConnected += OnPlayerConnect;
        }

        public void OnPlayerConnect(Entity player)
        {
            if (AntiHacker.HasBadName(player)) return;
            if (AntiHacker.HasBadIP(player)) AfterDelay(2000, () => {
                Utilities.ExecuteCommand($"kick \"{player.Name}\" ^1Proxies and VPNs are not allowed on this server!");
                return;
            });

            PlayerList.Add(player);
            Players.Add(player);

            player.SetClientDvar("cg_objectiveText", "^3This server is powered by ^1Nightingale ^5by Mahjestic.");
        }

        public void OnServerStart()
        {
            WriteLog.Info("Nightingale by Mahjestic");
        }
    }
}

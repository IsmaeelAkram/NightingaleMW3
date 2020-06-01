using InfinityScript;
using System.IO;

namespace Nightingale
{
    public class Nightingale : BaseScript
    {
        public Nightingale()
        {
            OnServerStart();
            PlayerConnected += OnPlayerConnect;
        }

        public void OnPlayerConnect(Entity player)
        {
            if (AntiHacker.HasBadName(player)) AfterDelay(2000, () => {
                Moderation.KickPlayer(player, Messages.BadName);
                WriteLog.Warning($"{player.Name} has been kicked for a bad name.");
                return;
            });
            if (AntiHacker.HasBadIP(player)) AfterDelay(2000, () => {
                Moderation.KickPlayer(player, Messages.BadIP);
                WriteLog.Warning($"{player.Name} has been kicked for a bad IP.");
                return;
            });

            Players.Add(player);

            player.SetClientDvar("cg_objectiveText", "^3This server is powered by ^1Nightingale ^5by Mahjestic.");
        }

        public void OnServerStart()
        {
            if(!Directory.Exists(Paths.MainPath)) Directory.CreateDirectory(Paths.MainPath);
            if (!Directory.Exists(Paths.AntiHackerPath)) Directory.CreateDirectory(Paths.AntiHackerPath);

            WriteLog.Info("Nightingale started.");
        }
    }
}

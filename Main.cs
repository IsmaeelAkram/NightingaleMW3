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
            PlayerDisconnected += OnPlayerDisconnect;
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
                WriteLog.Warning($"{player.Name} has been kicked for a bad IP (VPN, proxy).");
                return;
            });
            if (AntiHacker.HasInvalidID(player)) AfterDelay(2000, () => {
                Moderation.KickPlayer(player, Messages.BadIP);
                WriteLog.Warning($"{player.Name} has been kicked for a bad ID (HWID,GUID,UID).");
                return;
            });

            Players.Add(player);

            player.SetClientDvar("cg_objectiveText", "^3This server is powered by ^1Nightingale^3.");
        }

        public void OnPlayerDisconnect(Entity player)
        {

        }

        public override EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            if (!message.StartsWith("!")) return EventEat.EatGame;

            Commands.ProcessCommand(player, name, message);

            return EventEat.EatGame;
        }

        public void OnServerStart()
        {
            if(!Directory.Exists(Paths.MainPath)) Directory.CreateDirectory(Paths.MainPath);
            if (!Directory.Exists(Paths.AntiHackerPath)) Directory.CreateDirectory(Paths.AntiHackerPath);

            Commands.InitCommands();

            WriteLog.Info("Nightingale started.");
        }
    }
}

using InfinityScript;
using System.IO;

namespace Nightingale
{
    public partial class Nightingale : BaseScript
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
                KickPlayer(player, Config.GetString("bad_name"));
                WriteLog.Warning($"{player.Name} has been kicked for a bad name.");
                return;
            });
            if (AntiHacker.HasBadIP(player)) AfterDelay(2000, () => {
                KickPlayer(player, Config.GetString("bad_ip"));
                WriteLog.Warning($"{player.Name} has been kicked for a bad IP (VPN, proxy).");
                return;
            });
            if (AntiHacker.HasInvalidID(player)) AfterDelay(2000, () => {
                KickPlayer(player, Config.GetString("bad_id"));
                WriteLog.Warning($"{player.Name} has been kicked for a bad ID (HWID,GUID,UID).");
                return;
            });

            player.SetClientDvar("cg_objectiveText", "^3This server is powered by ^1Nightingale^3.");
            player.SetClientDvar("waypointIconWidth", "1");
            player.SetClientDvar("waypointIconHeight", "1");
        }

        public void OnPlayerDisconnect(Entity player)
        {

        }

        public override EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            if (!message.StartsWith("!"))
            {
                WriteLog.Info($"{player.Name}: {message}");
                Utilities.RawSayAll($"{player.Name}: {message}");
                return EventEat.EatGame;
            }

            ProcessCommand(player, name, message);

            return EventEat.EatGame;
        }

        public void OnServerStart()
        {
            WriteLog.Info("Nightingale starting...");
            if (!Directory.Exists(Config.GetPath("main"))) Directory.CreateDirectory(Config.GetPath("main"));
            if (!Directory.Exists(Config.GetPath("anti_hacker"))) Directory.CreateDirectory(Config.GetPath("anti_hacker"));

            InitCommands();

            WriteLog.Info("Nightingale started.");
        }
    }
}

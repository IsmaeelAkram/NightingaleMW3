using InfinityScript;
using System.IO;

namespace Nightingale
{
    public partial class Nightingale : BaseScript
    {
        public Nightingale() : base()
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
        }

        public void OnPlayerDisconnect(Entity player)
        {

        }

        public override EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            if (!message.StartsWith("!"))
            {
                WriteLog.Info($"{player.Name}: {message}");
                SayToAll($"{player.Name}: {message}");
                return EventEat.EatGame;
            }

            ProcessCommand(player, name, message);

            return EventEat.EatGame;
        }

        public void OnServerStart()
        {
            if(!Directory.Exists(Config.GetString("main"))) Directory.CreateDirectory(Config.GetString("main"));
            if (!Directory.Exists(Config.GetString("anti_hacker"))) Directory.CreateDirectory(Config.GetString("anti_hacker"));

            InitCommands();

            WriteLog.Info("Nightingale started.");
        }
    }
}

using InfinityScript;
using System.Collections.Generic;
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
            if (AntiHacker.HasBadIP(player)) AfterDelay(2000, () =>
            {
                KickPlayer(player, Config.GetString("bad_ip"));
                WriteLog.Warning($"{player.Name} has been kicked for a bad ip (vpn, proxy).");
                return;
            });
            if (AntiHacker.HasInvalidID(player)) AfterDelay(2000, () =>
            {
                KickPlayer(player, Config.GetString("bad_id"));
                WriteLog.Warning($"{player.Name} has been kicked for a bad id (hwid,guid,uid).");
                return;
            });

            player.SetClientDvar("cg_objectiveText", "^3This server is powered by ^1Nightingale^3.");
            player.SetClientDvar("waypointIconWidth", "0");
            player.SetClientDvar("waypointIconHeight", "0");
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
            WriteLog.Info("Nightingale starting...");

            // Create directories
            WriteLog.Info("Creating Nightingale directories...");
            foreach (string directory in Config.Paths)
            {
                if (!Directory.Exists(directory.Value)) Directory.CreateDirectory(directory.Value);
            }
            // Create files
            WriteLog.Info("Creating Nightingale files...");
            foreach (string file in Config.Files)
            {
                if (!File.Exists(file)) File.Create(file);
            }

            InitCommands();
<<<<<<< HEAD
            //CreateDirsAndFiles();
=======
>>>>>>> parent of 539a061... Trying to fix no log error

            WriteLog.Info("Nightingale started.");
        }
    }
}

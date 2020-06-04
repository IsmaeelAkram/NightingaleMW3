using InfinityScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            WriteLog.Info($"{player.Name} connected.");

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



            //Check for config file
            if (File.Exists(Config.GetPath("players") + $"{player.HWID}.dat"))
            {
                string[] options = File.ReadAllLines(Config.GetPath("players") + $"{player.HWID}.dat");
                foreach (string option_ in options)
                {
                    string optionTrimmed = option_.Trim();
                    string[] option = optionTrimmed.Split(new string[] { " = " }, StringSplitOptions.None);

                    if (option[0] == "OriginalName")
                    {
                        player.SetField(option[0], option[1]);
                    }
                    if (option[0] == "Alias")
                    {
                        player.SetField(option[0], option[1].Replace("\"", ""));
                    }
                    if (option[0] == "GroupName")
                    {
                        player.SetField(option[0], option[1]);
                    }
                    if (option[0] == "Warns")
                    {
                        player.SetField(option[0], Int32.Parse(option[1]));
                    }
                }
            }
            else
            {
                WriteLog.Info($"Writing data to Players/{player.HWID}.dat");
                File.WriteAllText(Config.GetPath("players") + $"{player.HWID}.dat", File.ReadAllText(Config.GetPath("players") + "Default.dat").Replace("<name>", player.Name).Replace("<HWID>", player.HWID).Replace("<GUID>", player.GUID.ToString()).Replace("<XUID>", player.GetXUID()));
                WriteLog.Info($"Done");

                string[] options = File.ReadAllLines(Config.GetPath("players") + $"{player.HWID}.dat");
                foreach (string option_ in options)
                {
                    string optionTrimmed = option_.Trim();
                    string[] option = optionTrimmed.Split(new string[] { " = " }, StringSplitOptions.None);

                    if (option[0] == "OriginalName")
                    {
                        player.SetField(option[0], option[1]);
                    }
                    if (option[0] == "Alias")
                    {
                        string alias = option[1].Replace("\"", "");
                        player.SetField(option[0], alias);
                    }
                    if (option[0] == "GroupName")
                    {
                        player.SetField(option[0], option[1]);
                    }
                    if (option[0] == "Warns")
                    {
                        player.SetField(option[0], Int32.Parse(option[1]));
                    }
                }
            }

            string[] groupsFile = File.ReadAllLines(Config.GetFile("groups"));
            foreach (string group_ in groupsFile)
            {
                //RankName;RankTag;Commands
                string[] group = group_.Split(';');
                if (group[0] == (string)player.GetField("GroupName"))
                {
                    player.SetField("GroupPrefix", group[1]);
                    player.SetField("GroupAvailableCommands", group[2]);
                }
            }

            if (!((string)player.GetField("Alias") == "None"))
            {
                AfterDelay(2000, () => player.Name = player.GetField("Alias").ToString().Replace("\"", ""));
            }

            // Set dvars
            player.SetClientDvar("waypointIconWidth", "0");
            player.SetClientDvar("waypointIconHeight", "0");
            player.SetClientDvar("cg_objectiveText", $"^3This server is powered by ^1Nightingale {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}^3.");
        }

        public void OnPlayerDisconnect(Entity player)
        {

        }

        public override EventEat OnSay3(Entity player, ChatType type, string name, ref string message)
        {
            string[] groupsFile = File.ReadAllLines(Config.GetFile("groups"));
            foreach (string group_ in groupsFile)
            {
                //RankName;RankTag;Commands
                string[] group = group_.Split(';');
                if (group[0] == (string)player.GetField("GroupName"))
                {
                    if (!message.StartsWith("!"))
                    {
                        WriteLog.Info($"{(string)player.GetField("GroupPrefix")}{player.Name}: {message}");
                        Utilities.RawSayAll($"{(string)player.GetField("GroupPrefix")}{player.Name}:^7 {message}");
                        return EventEat.EatGame;
                    }
                    else
                    {
                        ProcessCommand(player, name, message, group);
                        return EventEat.EatGame;
                    }
                }
            }

            SayToPlayer(player, "^1You have an invalid group. Please tell an admin to fix this");

            return EventEat.EatGame;
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (inflictor.GetStance() == "prone")
            {
                WarnPlayer(inflictor, "dropshot");
            } else if (inflictor.PlayerAds() <= 60 && inflictor.PlayerAds() > 0) {
                WarnPlayer(inflictor, $"halfscope ({inflictor.PlayerAds()} percent)");
            }

            base.OnPlayerDamage(player, inflictor, attacker, damage, dFlags, mod, weapon, point, dir, hitLoc);
        }

        public void OnServerStart()
        {
            WriteLog.Info("Nightingale starting...");

            InitCommands();

            foreach (string path in Config.Paths.Values)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    continue;
                }
            }
            foreach (var file in Config.Files)
            {
                if (!File.Exists(file.Value))
                {
                    File.Create(file.Value);
                    WriteLog.Info($"Creating {file.Key} file");
                    Config.PutDefaultValues(file.Key);
                }
            }

            GSCFunctions.SetDvar("mapname", "^5iSnipe SND");

            WriteLog.Info("Nightingale started.");
        }
    }
}

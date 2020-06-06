using InfinityScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Nightingale
{
    public class Command
    {
        private Action<Entity, string[]> function;
        public string name;

        public Command(string name_, Action<Entity, string[]> function_)
        {
            function = function_;
            name = name_;
        }

        public void Run(Entity sender, string message)
        {
            string command = message.Substring(1).Split(' ')[0].ToLowerInvariant();
            string[] args = message.Substring(1).Replace(command, "").Trim().Split(' ');
            function(sender, args);
        }
    }

    public partial class Nightingale
    {
        public List<Command> CommandList = new List<Command>();

        public void InitCommands()
        {
            WriteLog.Info("Initializing commands...");


            CommandList.Add(new Command("help", (sender, args) =>
            {
                string helpMessage = "^3";
                if((string)sender.GetField("GroupAvailableCommands") == "*ALL*")
                {
                    foreach (Command cmd in CommandList)
                    {
                        helpMessage = helpMessage + cmd.name + ", ";
                    }
                }
                else
                {
                    foreach (string cmd in sender.GetField("GroupAvailableCommands").ToString().Split(','))
                    {
                        helpMessage = helpMessage + cmd + ", ";
                    }
                }
                SayToPlayer(sender, "^3Commands for ^1Nightingale^3:");
                SayToPlayer(sender, helpMessage);
            }));
           

            CommandList.Add(new Command("res", (sender, args) =>
            {
                Utilities.ExecuteCommand("fast_restart");
            }));

            CommandList.Add(new Command("map", (sender, args) =>
            {
                ChangeMap(args[0]);
            }));

            CommandList.Add(new Command("myalias", (sender, args) =>
            {
                string newAlias = String.Join(" ", args);
                if (newAlias == "")
                {
                    SetPlayerAlias(sender, (string)sender.GetField("Alias"), (string)sender.GetField("OriginalName"));
                    SayToPlayer(sender, FormatMessage(Config.GetString("alias_reset"), new Dictionary<string, string>()
                    {
                        {"target", (string)sender.GetField("OriginalName") }
                    }));
                    return;
                }
                else if(newAlias.Length > 15)
                {
                    SayToPlayer(sender, FormatMessage(Config.GetString("alias_invalid"), new Dictionary<string, string>()
                    {
                        {"var", newAlias }
                    }));
                    return;
                }
                else
                {
                    WriteLog.None((string)sender.GetField("Alias"));
                    SetPlayerAlias(sender, (string)sender.GetField("Alias"), newAlias);
                    SayToPlayer(sender, FormatMessage(Config.GetString("alias_success"), new Dictionary<string, string>()
                    {
                        {"target", (string)sender.GetField("OriginalName") },
                        {"var", newAlias }
                    }));
                }
            }));

            CommandList.Add(new Command("alias", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);
                string newAlias = String.Join(" ", args).Replace(args[0], "").Trim();
                if (newAlias == "")
                {
                    SetPlayerAlias(target, (string)target.GetField("Alias"), (string)target.GetField("OriginalName"));
                    SayToPlayer(sender, FormatMessage(Config.GetString("alias_reset"), new Dictionary<string, string>()
                    {
                        {"target", (string)target.GetField("OriginalName") }
                    }));
                    return;
                }
                else if (newAlias.Length > 15)
                {
                    SayToPlayer(sender, FormatMessage(Config.GetString("alias_invalid"), new Dictionary<string, string>()
                    {
                        {"var", newAlias }
                    }));
                    return;
                }
                else
                {
                    SetPlayerAlias(target, (string)target.GetField("Alias"), newAlias);
                    SayToPlayer(sender, FormatMessage(Config.GetString("alias_success"), new Dictionary<string, string>()
                    {
                        {"target", (string)sender.GetField("OriginalName") },
                        {"var", newAlias }
                    }));
                }
            }));

            CommandList.Add(new Command("cmd", (sender, args) =>
            {
                Utilities.ExecuteCommand(String.Join(" ", args));
            }));

            CommandList.Add(new Command("yell", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);
                if(target == null)
                {
                    SayToPlayer(sender, Config.GetString("player_not_found"));
                }
                target.IPrintLnBold(String.Join(" ", args).Replace(args[0], "").Trim());
            }));

            CommandList.Add(new Command("afk", (sender, args) =>
            {
                SetPlayerTeam(sender, "spectator");
            }));

            CommandList.Add(new Command("afk", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);
                SetPlayerTeam(target, "spectator");
            }));

            CommandList.Add(new Command("setgroup", (sender, args) => {
                Entity target = FindSinglePlayer(args[0]);
                string newGroup = args[1].ToLower();

                string[] groupsFile = File.ReadAllLines(Config.GetFile("groups"));

                foreach(string group_ in groupsFile)
                {
                    //RankName;RankTag;Commands
                    string[] group = group_.Split(';');
                    if (group[0] == newGroup)
                    {
                        SetPlayerGroup(target, (string)target.GetField("GroupName"), newGroup, group[1], group[2]);
                        SayToPlayer(sender, FormatMessage(Config.GetString("group_change_success"), new Dictionary<string, string>()
                        {
                            {"target", (string)target.GetField("OriginalName") },
                            {"var", newGroup }
                        }));
                        return;
                    }
                }
                SayToPlayer(sender, FormatMessage(Config.GetString("group_not_found"), new Dictionary<string, string>()
                {
                    {"var", newGroup }
                }));
            }));

            CommandList.Add(new Command("admins", (sender, args) =>
            {
                List<Entity> admins = new List<Entity>();
                foreach(Entity player in Players)
                {
                    if((string)player.GetField("GroupName") != "default")
                    {
                        admins.Add(player);
                    }
                }

                foreach(Entity admin in admins)
                {
                    SayToPlayer(sender, $"{(string)admin.GetField("GroupPrefix")}{(string)admin.GetField("OriginalName")}");
                }
            }));

            CommandList.Add(new Command("warn", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);
                string reason = String.Join(" ", args).Replace(args[0], "").Trim();

                WarnPlayer(target, reason, sender);
            }));

            CommandList.Add(new Command("unwarn", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);

                UnwarnPlayer(target, sender);
            }));

            CommandList.Add(new Command("kick", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);

                string reason = String.Join(" ", args).Replace(args[0], "").Trim();
                KickPlayer(target, reason, sender);
            }));

            CommandList.Add(new Command("ban", (sender, args) => 
            {
                Entity target = FindSinglePlayer(args[0]);
                string reason = String.Join(" ", args).Replace(args[0], "").Trim();

                PermBanPlayer(target, reason, sender);
            }));

            CommandList.Add(new Command("tempban", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);
                int minutes = Int32.Parse(args[1]);
                string reason = String.Join(" ", args).Replace(args[0], "").Replace(args[1], "").Trim();

                TempBanPlayer(target, minutes, reason, sender);
            }));

            CommandList.Add(new Command("unban", (sender, args) =>
            {
                string target = args[0];

                UnbanPlayer(target, sender);
            }));

            CommandList.Add(new Command("fakeban", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);
                string reason = String.Join(" ", args).Replace(args[0], "").Trim();

                SayToAll(FormatMessage(Config.GetString("ban_message"), new Dictionary<string, string>()
                    {
                        {"target", (string)target.Name },
                        { "instigator", sender.Name },
                        {"reason", reason }
                    }));
            }));

            WriteLog.Info("Initialized commands.");
        }

        public Command FindCommand(string cmdname)
        {
            foreach (Command cmd in CommandList)
                if (cmd.name == cmdname)
                    return cmd;
            return null;
        }

        public void ProcessCommand(Entity sender, string name, string message, string[] group)
        {
            string commandname = message.Substring(1).Split(' ')[0].ToLowerInvariant();
            WriteLog.Info(sender.Name + " attempted " + commandname);

            Command commandToBeRun;
            commandToBeRun = FindCommand(commandname);

            if (commandToBeRun == null)
            {
                SayToPlayer(sender, Config.GetString("unknown_cmd"));
            }
            else if (!group[2].Contains(commandToBeRun.name) && !group[2].Contains("*ALL*"))
            {
                SayToPlayer(sender, "Insufficient privileges.");
                return;
            }
            else
            {
                commandToBeRun.Run(sender, message);
            }
        }
    }
}

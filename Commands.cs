using InfinityScript;
using System;
using System.Collections.Generic;

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



            CommandList.Add(new Command("ping", (sender, args) =>
            {
                SayToPlayer(sender, "^1Pong!");
            }));

            CommandList.Add(new Command("help", (sender, args) =>
            {
                string helpMessage = "^3";
                foreach(Command cmd in CommandList)
                {
                    helpMessage = helpMessage + cmd.name + ", ";
                }
                SayToPlayer(sender, "^3Commands for ^1Nightingale^3:");
                SayToPlayer(sender, helpMessage);
            }));
            
            CommandList.Add(new Command("kick", (sender, args) =>
            {
                Entity target = FindSinglePlayer(args[0]);

                string reason = String.Join(" ", args).Replace(args[0], "").Trim();
                if(reason == "")
                {
                    reason = "no reason";
                }
                KickPlayer(target, reason);
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
                string alias = String.Join(" ", args).Trim();
                if (alias == "")
                {
                    alias = sender.Name;
                    SayToPlayer(sender, FormatMessage(Config.GetString("alias_success"), new Dictionary<string, string>() {
                        { "var", alias }
                    }));
                    return;
                }
                sender.Name = alias;
                SayToPlayer(sender, FormatMessage(Config.GetString("alias_success"), new Dictionary<string, string>() {
                        { "var", alias }
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

        public void ProcessCommand(Entity sender, string name, string message)
        {
            string commandname = message.Substring(1).Split(' ')[0].ToLowerInvariant();
            WriteLog.Info(sender.Name + " attempted " + commandname);

            Command commandToBeRun;
            commandToBeRun = FindCommand(commandname);
            commandToBeRun.Run(sender, message);
        }
    }
}

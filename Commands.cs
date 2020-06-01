using InfinityScript;
using System;
using System.Collections.Generic;

namespace Nightingale
{
    public class Command
    {
        private Action<Entity, string[], string> function;
        public string name;

        public Command(string name_, Action<Entity, string[], string> function_)
        {
            function = function_;
            name = name_;
        }

        public void Run(Entity sender, string message)
        {
            string command = message.Substring(1).Split(' ')[0].ToLowerInvariant();
            string[] args = message.Substring(1).Split(' ');
            function(sender, args, command);
        }
    }

    public partial class Nightingale
    {
        public List<Command> CommandList = new List<Command>();

        public void InitCommands()
        {
            WriteLog.Info("Initializing commands...");



            CommandList.Add(new Command("ping", (sender, args, command) =>
            {
                PrivateMessage(sender, "^1Pong!");
            }));

            CommandList.Add(new Command("help", (sender, args, command) =>
            {
                string helpMessage = "^3";
                foreach(Command cmd in CommandList)
                {
                    helpMessage = helpMessage + cmd.name + ", ";
                }
                PrivateMessage(sender, "^3Commands for ^1Nightingale^3:");
                PrivateMessage(sender, helpMessage);
            }));

            CommandList.Add(new Command("kick", (sender, args, command) =>
            {
                Entity target = FindSinglePlayer(args[1]);
                string reason = String.Join(" ", args).Replace(command, "");
                KickPlayer(target, reason);
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

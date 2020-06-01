using InfinityScript;
using System;
using System.Collections.Generic;

namespace Nightingale
{
    class Command
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

    static class Commands
    {
        public static List<Command> CommandList = new List<Command>();

        public static void InitCommands()
        {
            WriteLog.Info("Initializing commands...");



            CommandList.Add(new Command("ping", (sender, args, command) =>
            {
                Chat.SayToPlayer(sender, "^1Pong!");
            }));

            CommandList.Add(new Command("vpn", (sender, args, command) =>
            {
                string IP = args[1];
            }));


            WriteLog.Info("Initialized commands...");
        }

        public static Command FindCommand(string cmdname)
        {
            foreach (Command cmd in CommandList)
                if (cmd.name == cmdname)
                    return cmd;
            return null;
        }

        public static void ProcessCommand(Entity sender, string name, string message)
        {
            string commandname = message.Substring(1).Split(' ')[0].ToLowerInvariant();
            WriteLog.Info(sender.Name + " attempted " + commandname);

            Command commandToBeRun;
            commandToBeRun = FindCommand(commandname);
            commandToBeRun.Run(sender, message);
        }
    }
}

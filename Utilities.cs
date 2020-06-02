using InfinityScript;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nightingale
{
    public static class WriteLog
    {
        public static void None(string message)
        {
            Log.Write(LogLevel.None, message);
        }
        public static void Info(string message)
        {
            Log.Write(LogLevel.Info, message);
        }

        public static void Error(string message)
        {
            Log.Write(LogLevel.Error, message);
        }

        public static void Warning(string message)
        {
            Log.Write(LogLevel.Warning, message);
        }
    }

    public partial class Nightingale
    {
        public static void Announce(string message)
        {
            Utilities.RawSayAll(Config.GetString("announcement_prefix") + message);
        }
        public static void PrivateMessage(Entity player, string message)
        {
            Utilities.RawSayTo(player, Config.GetString("pm_prefix") + message);
        }

        public static void SayToAll(string message)
        {
            Utilities.RawSayAll(message);
        }
        public static void SayToPlayer(Entity player, string message)
        {
            Utilities.RawSayTo(player, message);
        }

        public static void ChangeMap(string mapName)
        {
            Utilities.ExecuteCommand($"map mp_{mapName}");
        }

        public static void KickPlayer(Entity player, string reason)
        {
            Utilities.ExecuteCommand($"kick \"{player.Name}\" {reason}");
            WriteLog.Info($"{player.Name} has been kicked for {reason}.");
            Announce(Config.GetString("kick_message"));
        }

        public List<Entity> FindPlayers(string identifier, Entity sender = null)
        {
            if (identifier.StartsWith("#"))
            {
                try
                {
                    int number = int.Parse(identifier.Substring(1));
                    Entity ent = Entity.GetEntity(number);
                    if (number >= 0 && number < 18)
                    {
                        foreach (Entity player in Players)
                        {
                            if (player.GetEntityNumber() == number)
                                return new List<Entity>() { ent };
                        }
                    }
                    return new List<Entity>();
                }
                catch (Exception)
                {
                }
            }
            identifier = identifier.ToLowerInvariant();
            return (from player in Players
                    where player.Name.ToLowerInvariant().Contains(identifier)
                    select player).ToList();
        }

        public Entity FindSinglePlayer(string identifier)
        {
            List<Entity> players = FindPlayers(identifier);
            if (players.Count != 1)
                return null;
            return players[0];
        }

    }
}

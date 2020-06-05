using InfinityScript;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static void SayToAll(string message)
        {
            Utilities.RawSayAll(Config.GetString("announcement_prefix") + message);
        }
        public static void SayToPlayer(Entity player, string message)
        {
            Utilities.RawSayTo(player, Config.GetString("pm_prefix") + message);
        }

        public static void ChangeMap(string mapName)
        {
            Utilities.ExecuteCommand($"map mp_{mapName}");
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
                    where player.GetField("OriginalName").ToString().ToLowerInvariant().Contains(identifier)
                    select player).ToList();
        }

        public Entity FindSinglePlayer(string identifier)
        {
            List<Entity> players = FindPlayers(identifier);
            if (players.Count != 1)
                return null;
            return players[0];
        }

        public void SetPlayerAlias(Entity player, string old_alias, string alias)
        {
            player.SetField("Alias", alias);
            player.Name = alias;

            String oldConfig = File.ReadAllText(Config.GetPath("players") + $"{player.HWID}.dat");
            File.WriteAllText(Config.GetPath("players") + $"{player.HWID}.dat", oldConfig.Replace($"Alias={old_alias}", $"Alias={alias}"));
        }

        public void SetPlayerGroup(Entity player, string old_group, string group, string groupPrefix, string groupAvailableCommands)
        {
            player.SetField("GroupName", group);
            player.SetField("GroupPrefix", groupPrefix);
            player.SetField("GroupAvailableCommands", groupAvailableCommands);

            String oldConfig = File.ReadAllText(Config.GetPath("players") + $"{player.HWID}.dat");
            File.WriteAllText(Config.GetPath("players") + $"{player.HWID}.dat", oldConfig.Replace($"Group={old_group}", $"Group={group}"));
        }

        public void KickPlayer(Entity player, string reason, Entity inflictor = null)
        {
            Utilities.ExecuteCommand($"kick \"{player.GetField("OriginalName")}\" {reason}");
            WriteLog.Info($"{player.GetField("OriginalName")} has been kicked for {reason}.");

            if (inflictor == null)
            {
                SayToAll(FormatMessage(Config.GetString("kick_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", "Nightingale" },
                    { "reason", reason }
                }));
            }
            else
            {
                SayToAll(FormatMessage(Config.GetString("kick_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", inflictor.Name },
                    { "reason", reason }
                }));
            }
        }

        public void UnbanPlayer(string player, Entity inflictor = null)
        {
            Utilities.ExecuteCommand($"unban \"{player}\"");
            WriteLog.Info($"{player} has been unbanned.");

            if(inflictor == null)
            {
                SayToAll(FormatMessage(Config.GetString("unban_message"), new Dictionary<string, string>()
                {
                    { "target", player },
                    { "instigator", "Nightingale" }
                }));
            }
            else
            {
                SayToAll(FormatMessage(Config.GetString("unban_message"), new Dictionary<string, string>()
                {
                    { "target", player },
                    { "instigator", inflictor.Name }
                }));
            }
        }

        public void BanPlayer(Entity player, string reason, Entity inflictor = null)
        {
            Utilities.ExecuteCommand($"banclient \"{player.GetEntityNumber()}\" {reason}");
            WriteLog.Info($"{player.GetField("OriginalName")} has been banned for {reason}.");

            if (inflictor == null)
            {
                SayToAll(FormatMessage(Config.GetString("ban_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", "Nightingale" },
                    { "reason", reason }
                }));
            }
            else
            {
                SayToAll(FormatMessage(Config.GetString("ban_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", inflictor.Name },
                    { "reason", reason }
                }));
            }
        }

        public void UnwarnPlayer(Entity player, Entity inflictor = null)
        {
            int oldWarns = (int)player.GetField("Warns");
            player.SetField("Warns", oldWarns - 1);
            int newWarns = (int)player.GetField("Warns");

            String oldConfig = File.ReadAllText(Config.GetPath("players") + $"{player.HWID}.dat");
            File.WriteAllText(Config.GetPath("players") + $"{player.HWID}.dat", oldConfig.Replace($"Warns={oldWarns}", $"Warns={newWarns}"));

            if (inflictor == null)
            {
                SayToAll(FormatMessage(Config.GetString("unwarn_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", "Nightingale" },
                    { "warns", newWarns.ToString() },
                    { "maxWarns", "3" }
                }));
            }
            else
            {
                SayToAll(FormatMessage(Config.GetString("unwarn_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", inflictor.Name },
                    { "warns", newWarns.ToString() },
                    { "maxWarns", "3" }
                }));
            }
        }

        public void WarnPlayer(Entity player, string reason, Entity inflictor = null)
        {
            int oldWarns = (int)player.GetField("Warns");
            player.SetField("Warns", oldWarns + 1);
            int newWarns = (int)player.GetField("Warns");

            String oldConfig = File.ReadAllText(Config.GetPath("players") + $"{player.HWID}.dat");
            File.WriteAllText(Config.GetPath("players") + $"{player.HWID}.dat", oldConfig.Replace($"Warns={oldWarns}", $"Warns={newWarns}"));

            if ((int)player.GetField("Warns") == 3)
            {
                // TODO Change to temp-ban
                if (inflictor == null)
                {
                    KickPlayer(player, "^1Too many warnings.");
                    player.SetField("Warns", 0);
                    oldConfig = File.ReadAllText(Config.GetPath("players") + $"{player.HWID}.dat");
                    File.WriteAllText(Config.GetPath("players") + $"{player.HWID}.dat", oldConfig.Replace($"Warns={(string)player.GetField("Warns")}", "Warns=0"));
                }
                else
                {
                    KickPlayer(player, "^1Too many warnings.", inflictor);
                    player.SetField("Warns", 0);
                    oldConfig = File.ReadAllText(Config.GetPath("players") + $"{player.HWID}.dat");
                    File.WriteAllText(Config.GetPath("players") + $"{player.HWID}.dat", oldConfig.Replace($"Warns={(string)player.GetField("Warns")}", "Warns=0"));
                }
            }

            if(inflictor == null)
            {
                SayToAll(FormatMessage(Config.GetString("warn_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", "Nightingale" },
                    { "reason", reason },
                    { "warns", newWarns.ToString() },
                    { "maxWarns", "3" }
                }));
            }
            else
            {
                SayToAll(FormatMessage(Config.GetString("warn_message"), new Dictionary<string, string>()
                {
                    { "target", player.Name },
                    { "instigator", inflictor.Name },
                    { "reason", reason },
                    { "warns", newWarns.ToString() },
                    { "maxWarns", "3" }
                }));
            }
        }
    }
}

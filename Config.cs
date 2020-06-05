using System;
using System.Collections.Generic;
using System.IO;

namespace Nightingale
{
    public class Config
    {
        public static Dictionary<string, bool> DefaultSettings = new Dictionary<string, bool>()
        {
            {"hidebombicons", true },
        };

        public static Dictionary<string, string> DefaultLang = new Dictionary<string, string>()
        {
            {"unknown_cmd", "^5Unknown command." },
            {"kick_message", "^7<target>^5 has been kicked by ^7<instigator>^5 for ^7<reason>."},
            {"ban_message", "^7<target>^5 has been banned by ^7<instigator>^5 for ^7<reason>."},
            {"tempban_message", "^7<target>^5 has been temporarily banned by ^7<instigator>^5 for ^7<reason>."},
            {"banned_message", "^1You are banned from this server."},
            {"tempbanned_message", "^1You are temporarily banned from this server."},
            {"unban_message", "^7<target>^5 has been unbanned by ^7<instigator>^5."},
            {"unban_entry_not_found", "Player <target> was not found in the ban entries." },
            {"warn_success", "^7<target>^5 has been warned (^7<warns>^5/^7<maxwarns>^5) by ^7<instigator>^5 for ^7<reason>."},
            {"unwarn_success", "^7<target>^5 has been unwarned (^7<warns>^5/^7<maxwarns>^5) by ^7<instigator>^5."},
            {"bad_ip", "^1Proxies and VPNs are not allowed on this server!"},
            {"bad_name", "^1Bye hacker!"},
            {"bad_id", "^1Bye hacker!"},
            {"alias_success", "^5Changed ^7<target>'s^5 alias to ^7<var>." },
            {"alias_reset", "^5Reset ^7<target>'s^5 alias." },
            {"alias_invalid", "^7<var> ^5is invalid. ^7(Too long or has invalid characters)" },
            {"group_change_success", "^5Changed ^7<target>'s ^5group to ^7<var>^5." },
            {"group_not_found", "^1Group <var> not found." },

            {"announcement_prefix", "^7[^5Nightingale^7] "},
            { "pm_prefix", "^0(^2PM^0)^7 "}
        };

        public static Dictionary<string, string> Paths = new Dictionary<string, string>()
        {
            { "main","scripts\\Nightingale\\" },
            { "anti_hacker", "scripts\\Nightingale\\AntiHacker\\" },
            { "utils", "scripts\\Nightingale\\Utils\\" },
            { "players", "scripts\\Nightingale\\Players\\" }
        };

        public static Dictionary<string, string> Files = new Dictionary<string, string>()
        {
            { "lang", GetPath("main") + "lang.txt" },
            { "settings", GetPath("main") + "settings.txt" },
            { "groups", GetPath("main") + "groups.txt" },
            { "bad_names", GetPath("anti_hacker") + "badnames.txt" },
            { "bad_ips", GetPath("anti_hacker") + "badIPs.txt" },
            { "banned_players", GetPath("utils") + "bannedplayers.txt" }
        };

        public static bool GetBool(string name)
        {
            return DefaultSettings[name];
        }

        public static string GetString(string name)
        {
            return DefaultLang[name];
        }

        public static string GetPath(string name)
        {
            return Paths[name];
        }

        public static string GetFile(string name)
        {
            return Files[name];
        }

        public static void PutDefaultValues(string fileKey)
        {
            WriteLog.Info("Putting default values...");
            if (fileKey == "settings")
            {
                WriteLog.Info("Putting default values for settings.");
                foreach (var setting in DefaultSettings)
                {
                    WriteLog.Info($"Writing {setting.Key} to settings.");
                    File.AppendAllText(GetFile("settings"), $"{setting.Key}={setting.Value}" + Environment.NewLine);
                }
                return;
            }
            if (fileKey == "lang")
            {
                WriteLog.Info("Putting default values for lang.");
                foreach (var lang in DefaultLang)
                {
                    WriteLog.Info($"Writing {lang.Key} to lang.");
                    File.AppendAllText(GetFile("lang"), $"{lang.Key}={lang.Value}" + Environment.NewLine);
                }
                return;
            }
            else
            {
                WriteLog.Info($"{fileKey} does not have any specified default values.");
                return;
            }
        }
    }

    public partial class Nightingale
    {
        public string FormatMessage(string str, Dictionary<string, string> dict)
        {
            foreach (KeyValuePair<string, string> pair in dict)
                str = str.Replace("<" + pair.Key + ">", pair.Value);
            return str;
        }
    }
}

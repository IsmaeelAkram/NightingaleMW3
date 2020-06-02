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
            {"bad_ip", "^1Proxies and VPNs are not allowed on this server!"},
            {"bad_name", "^1Bye hacker!"},
            {"bad_id", "^1Bye hacker!"},
            {"alias_success", "^5Changed alias to ^7<var>." },
            {"alias_too_long", "^7<var> ^5is over 15 characters." },

            {"announcement_prefix", "^7[^5Nightingale^7] "},
            { "pm_prefix", "^7[^5PM^7] "}
        };

        public static Dictionary<string, string> Paths = new Dictionary<string, string>()
        {
            { "main","scripts\\Nightingale\\" },
            { "anti_hacker", "scripts\\Nightingale\\AntiHacker\\" },
            { "utils", "scripts\\Nightingale\\Utils\\" }
        };

        public static Dictionary<string, string> Files = new Dictionary<string, string>()
        {
            { "lang", GetPath("main") + "lang.txt" },
            { "settings", GetPath("main") + "settings.txt" },
            { "bad_names", GetPath("anti_hacker") + "badnames.txt" },
            { "bad_ips", GetPath("anti_hacker") + "badIPs.txt" },
            { "aliases", GetPath("utils") + "aliases.txt" },
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
        public string FormatMessage(string message, Dictionary<string, string> dict)
        {
            string newMessage = "";
            try { newMessage = message.Replace("<target>", dict["target"]); } catch { }
            try { newMessage = message.Replace("<instigator>", dict["instigator"]); } catch { }
            try { newMessage = message.Replace("<reason>", dict["reason"]); } catch { }
            try { newMessage = message.Replace("<var>", dict["var"]); } catch { }

            return newMessage;
        }
    }
}

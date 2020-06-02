using System.Collections.Generic;

namespace Nightingale
{
    public static class Config
    {
        public static List<string> Files = new List<string>()
        {
            GetPath("anti_hacker") + "badnames.txt",
            GetPath("anti_hacker") + "badIPs.txt"
        };

        public static Dictionary<string, string> Paths = new Dictionary<string, string>()
        {
            { "main","scripts\\Nightingale\\" },
            { "anti_hacker", "scripts\\Nightingale\\AntiHacker\\" },
            { "utils", "scripts\\Nightingale\\Utils\\" }
        };



        public static Dictionary<string, bool> DefaultSettings = new Dictionary<string, bool>()
        {
            {"hidebombicons", true },
        };

        public static Dictionary<string, string> DefaultLang = new Dictionary<string, string>()
        {
            {"kick_message", "^7<target>^5 has been kicked by ^7<instigator>^5 for ^7<reason>."},
            {"ban_message", "^7<target>^5 has been banned by ^7<instigator>^5 for ^7<reason>."},
            {"bad_ip", "^1Proxies and VPNs are not allowed on this server!"},
            {"bad_name", "^1Bye hacker!"},
            {"bad_id", "^1Bye hacker!"},
            {"alias_success", "^5Changed alias to ^7<var>." },
            {"alias_too_long", "^7<var> ^5is over 15 characters. Not a valid alias." },

            {"announcement_prefix", "^7[^5Nightingale^7] "},
            { "pm_prefix", "^7[^5PM^7] "}
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

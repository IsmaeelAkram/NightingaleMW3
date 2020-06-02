using InfinityScript;
using System.IO;
using Tiny.RestClient;

namespace Nightingale
{
    class AntiHacker
    {
        //TODO Check for invalid HWID,GUID,XUID
        //TODO Check for VPN

        public static bool HasBadName(Entity player)
        {
            if (File.Exists(Config.GetString("anti_hacker") + "badnames.txt"))
            {
                string[] names = File.ReadAllLines(Config.GetString("anti_hacker") + "badnames.txt");
                foreach(string name in names)
                {
                    if(player.Name == name)
                    {
                        return true;
                    }
                }
            }
            else
            {
                File.Create(Config.GetString("anti_hacker") + "badnames.txt");
            }
            return false;
        }

        public static bool HasBadIP(Entity player)
        {
            if (File.Exists(Config.GetString("anti_hacker") + "badIPs.txt"))
            {
                string[] IPs = File.ReadAllLines(Config.GetString("anti_hacker") + "badIPs.txt");
                foreach (string IP in IPs)
                {
                    if (player.IP.Address.ToString() == IP)
                    {
                        return true;
                    }
                }
            }
            else
            {
                File.Create(Config.GetString("anti_hacker") + "badIPs.txt");
            }
            return false;
        }

        public static bool HasInvalidID(Entity player)
        {
            string hwid = player.HWID;
            string guid = player.GUID.ToString();
            string uid = player.UserID.ToString();

            if(hwid.Trim() == "" || guid.Trim() == "" || uid.Trim() == "")
            {
                return true;
            }

            return false;
        }
    }
}

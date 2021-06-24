using InfinityScript;
using System.IO;
using Tiny.RestClient;

namespace Nightingale
{
    class AntiHacker
    {
        //TODO: Check for invalid HWID,GUID,XUID
        //TODO: Check for VPN

        public static bool HasBadName(Entity player)
        {
            string[] names = File.ReadAllLines(Config.GetFile("bad_names"));
            foreach(string name in names)
            {
                if(player.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasBadIP(Entity player)
        {
            string[] IPs = File.ReadAllLines(Config.GetFile("bad_ips"));
            foreach (string IP in IPs)
            {
                if (player.IP.Address.ToString() == IP)
                {
                    return true;
                }
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

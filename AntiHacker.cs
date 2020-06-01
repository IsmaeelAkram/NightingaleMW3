using InfinityScript;
using System.IO;
using Tiny.RestClient;

namespace Nightingale
{
    class AntiHacker
    {
        public static bool HasBadName(Entity player)
        {
            if (File.Exists(Paths.AntiHackerPath + "badnames.txt"))
            {
                string[] names = File.ReadAllLines(Paths.AntiHackerPath + "badnames.txt");
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
                File.Create(Paths.AntiHackerPath + "badnames.txt");
            }
            return false;
        }

        public static bool HasBadIP(Entity player)
        {
            if (File.Exists(Paths.AntiHackerPath + "badIPs.txt"))
            {
                string[] IPs = File.ReadAllLines(Paths.AntiHackerPath + "badIPs.txt");
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
                File.Create(Paths.AntiHackerPath + "badIPs.txt");
            }
            return false;
        }
    }
}

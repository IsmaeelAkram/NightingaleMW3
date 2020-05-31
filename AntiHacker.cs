using InfinityScript;
using System.IO;

namespace Nightingale
{
    class AntiHacker
    {
        public static bool HasBadName(Entity player)
        {
            if (File.Exists(Paths.ConfigPath + "Nightingale/AntiHacker/badnames.txt"))
            {
                string[] names = File.ReadAllLines(Paths.ConfigPath + "Nightingale/AntiHacker/badnames.txt");
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
                File.Create(Paths.ConfigPath + "Nightingale/AntiHacker/badnames.txt");
            }
            return false;
        }

        public static bool HasBadIP(Entity player)
        {
            return false;
        }
    }
}

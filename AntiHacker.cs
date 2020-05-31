using InfinityScript;
using System.IO;

namespace Nightingale
{
    class AntiHacker
    {
        public void CheckBadName(Entity player)
        {
            if (File.Exists(Paths.ConfigPath + "Nightingale/AntiHacker/badnames.txt"))
            {
                string[] names = File.ReadAllLines(Paths.ConfigPath + "Nightingale/AntiHacker/badnames.txt");

            }
        }
    }
}

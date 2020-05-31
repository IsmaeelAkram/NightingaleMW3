using InfinityScript;
using System.Collections.Generic;

namespace Nightingale
{
    public class Nightingale : BaseScript
    {
        List<Entity> PlayerList;

        public Nightingale()
        {
            OnServerStart();
            PlayerConnected += OnPlayerConnect;
        }

        public void OnPlayerConnect(Entity player)
        {
            PlayerList.Add(player);
            Players.Add(player);

            player.SetClientDvar("cg_objectiveText", "^3This server is powered by ^1Nightingale ^5by Mahjestic.");
        }

        public void OnServerStart()
        {

        }
    }
}

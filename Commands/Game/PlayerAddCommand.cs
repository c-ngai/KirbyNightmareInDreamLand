using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using System;

namespace KirbyNightmareInDreamLand.Commands
{
    public class PlayerAddCommand : ICommand
    {
        private ObjectManager objectManager;
        public PlayerAddCommand()
        {
            objectManager = ObjectManager.Instance;
        }

        public void Execute()
        {
            if (objectManager.Players.Count < Constants.Game.MAXIMUM_PLAYER_COUNT)
            {
                Vector2 player1Position = objectManager.Players[0].GetKirbyPosition();
                IPlayer kirby = new Player(player1Position);
                objectManager.Players.Add(kirby);
            }
        }
    }
}

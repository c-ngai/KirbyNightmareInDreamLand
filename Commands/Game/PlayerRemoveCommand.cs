using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using System;

namespace KirbyNightmareInDreamLand.Commands
{
    public class PlayerRemoveCommand : ICommand
    {
        private ObjectManager objectManager;
        public PlayerRemoveCommand()
        {
            objectManager = ObjectManager.Instance;
        }

        public void Execute()
        {
            if (objectManager.Players.Count > 1)
            {
                objectManager.RemoveDynamicObject(objectManager.Players[objectManager.Players.Count - 1] as ICollidable);
                objectManager.Players.RemoveAt(objectManager.Players.Count - 1);
            }
        }
    }
}

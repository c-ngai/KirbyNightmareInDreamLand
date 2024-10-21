﻿using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyJumpCommand : ICommand
    {
        public void Execute()
        {
            ObjectManager.Instance.Players[0].Jump();
        }
    }
}

﻿using MasterGame.Entities.Players;
namespace MasterGame.Commands
{
    public class KirbyTakeDamageCommand : ICommand
    { 
        private IPlayer kirby;

        public KirbyTakeDamageCommand(IPlayer player)
        {
            kirby = player;
        }

        public void Execute()
        {
            kirby.TakeDamage();
        }

        public void Undo()
        {
            kirby.StopMoving();
        }
    }
}

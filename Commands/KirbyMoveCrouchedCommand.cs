﻿using Microsoft.Xna.Framework.Input;
using System.Linq;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveCrouchedCommand : ICommand
    {
        private IPlayer kirby;
        private KeyboardController keyboard;
        private Keys crouchKey;
        private Keys attackKey;

        // Constructor with no parameters
        public KirbyMoveCrouchedCommand()
        {
            kirby = Game1.Instance.players[0];
            keyboard = Game1.Instance.keyboard;
            crouchKey = Keys.Down;
            attackKey = Keys.Z;
        }

        public void Execute()
        {
            kirby.Crouch();
            // if (keyboard.currentState.Contains(attackKey))
            // {
            //     keyboard.stopKeys[crouchKey].Execute();
            // }
        }

    }
}

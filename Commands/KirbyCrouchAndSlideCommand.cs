﻿using Microsoft.Xna.Framework.Input;
using System.Linq;
using KirbyNightmareInDreamLand.Time;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyCrouchAndSlideCommand : ICommand
    {
        private Game1 game;
        private IPlayer kirby;
        private bool isSliding;

        private KeyboardController keyboard;
        private Keys attackKey;
        private Keys crouchKey;

        private ITimeCalculator timer;
        private double startingTime;

        public KirbyCrouchAndSlideCommand(IPlayer player, Keys keyCrouch, Keys keyAttack, KeyboardController currentKeyboard)
        {
            kirby = player;
            crouchKey = keyCrouch;
            attackKey = keyAttack;
            keyboard = currentKeyboard;
            timer = new TimeCalculator();
            game = Game1.Instance;
            startingTime = 0;
            isSliding = false;
        }
        public void Execute()
        {
            //Determines if a timer needs to be set to keep track of slide time when the attack key is also pressed and if the timer needs to be reset
            if (keyboard.currentState.Contains(attackKey) && startingTime == 0)
            {
                startingTime = timer.GetCurrentTimeInMS(game.time);
            }
            else if (keyboard.currentState.Contains(attackKey) && startingTime != 0 && (timer.GetCurrentTimeInMS(game.time) - startingTime < Constants.Controller.SLIDE_TIME))
            {
                isSliding = true;
            }
            else
            {
                startingTime = 0;
                isSliding = false;
            }

            if (isSliding)
            {
                kirby.Slide();
            }
            else if (!keyboard.currentState.Contains(crouchKey))
            {
                kirby.EndCrouch();
            }
        }
    }
}
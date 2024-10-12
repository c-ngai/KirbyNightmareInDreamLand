using Microsoft.Xna.Framework.Input;
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

        private ITimeCalculator timer;
        private double startingTime;

        public KirbyCrouchAndSlideCommand(IPlayer player, Keys keyMapped, KeyboardController currentKeyboard, Game1 currentGame)
        {
            kirby = player;
            attackKey = keyMapped;
            keyboard = currentKeyboard;
            timer = new TimeCalculator();
            game = currentGame;
            startingTime = 0;
            isSliding = false;
        }
        public void Execute()
        {
            // Determines if a timer needs to be set to keep track of slide time when the attack key is also pressed and if the timer needs to be reset
            // if (keyboard.currentState.Contains(attackKey) && startingTime == 0)
            // {
            //     startingTime = timer.GetCurrentTimeInMS(game.time);
            //     isSliding = false;
            // }
            // else if (startingTime != 0 && timer.GetCurrentTimeInMS(game.time) - startingTime < Constants.Controller.SLIDE_TIME)
            // {
            //     isSliding = true;
            // }
            // else
            // {
            //     startingTime = 0;
            //     isSliding = false;
            // }

            // if (isSliding)
            // {
            //     kirby.Slide();
            // }
            // else
            // {
                kirby.Crouch();
            //}
        }
    }
}
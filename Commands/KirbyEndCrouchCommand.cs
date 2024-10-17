using Microsoft.Xna.Framework.Input;
using System.Linq;
using KirbyNightmareInDreamLand.Time;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyEndCrouchCommand : ICommand
    {
        private IPlayer kirby;
        private bool isSliding;

        private KeyboardController keyboard;
        private Keys attackKey;
        private Keys crouchKey;

        private ITimeCalculator timer;
        private double startingTime;

        public KirbyEndCrouchCommand()
        {
            // Accessing the player and keyboard controller through Game1.Instance
            kirby = Game1.Instance.players[0]; // Assuming there is always at least one player
            keyboard = Game1.Instance.keyboard;
            crouchKey = Keys.Down; // Set the crouch key to down arrow
            attackKey = Keys.Z;    // Set the attack key to 'Z'
            timer = new TimeCalculator();
            startingTime = 0;
            isSliding = false;
        }

        public void Execute()
        {
            // Determines if a timer needs to be set to keep track of slide time when the attack key is also pressed and if the timer needs to be reset
            // if (keyboard.currentState.Contains(attackKey) && startingTime == 0)
            // {
            //     startingTime = timer.GetCurrentTimeInMS(Game1.Instance.time);
            // }
            // else if (keyboard.currentState.Contains(attackKey) && startingTime != 0 && (timer.GetCurrentTimeInMS(Game1.Instance.time) - startingTime < Constants.Controller.SLIDE_TIME))
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
            // // }
            // else if (!keyboard.currentState.Contains(crouchKey))
            // {
            //     kirby.EndCrouch();
            // }
            kirby.EndCrouch();
        }
    }
}

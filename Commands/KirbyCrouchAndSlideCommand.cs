using Microsoft.Xna.Framework.Input;
using System.Linq;
using KirbyNightmareInDreamLand.Time;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyCrouchAndSlideCommand : ICommand
    {
        private IPlayer kirby;
        private bool isSliding;

        private KeyboardController keyboard;
        private Keys attackKey;
        private Keys crouchKey;

        private ITimeCalculator timer;
        private double startingTime;

        public KirbyCrouchAndSlideCommand()
        {

            kirby = Game1.Instance.players[0]; // Assuming there is one player
            keyboard = Game1.Instance.KeyboardController;
            crouchKey = Keys.Down; 
            attackKey = Keys.Z; 
            timer = new TimeCalculator();
            startingTime = 0;
            isSliding = false;
        }

        public void Execute()
        {
            if (keyboard.currentState.Contains(attackKey) && startingTime == 0)
            {
                startingTime = timer.GetCurrentTimeInMS(Game1.Instance.time);
            }
            else if (keyboard.currentState.Contains(attackKey) && startingTime != 0 && (timer.GetCurrentTimeInMS(Game1.Instance.time) - startingTime < Constants.Controller.SLIDE_TIME))
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
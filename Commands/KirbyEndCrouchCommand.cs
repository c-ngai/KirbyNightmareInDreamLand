using Microsoft.Xna.Framework.Input;
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
            kirby = ObjectManager.Instance.Players[0]; // Assuming there is always at least one player
            keyboard = Game1.Instance.Keyboard;
            crouchKey = Keys.Down; // Set the crouch key to down arrow
            attackKey = Keys.Z;    // Set the attack key to 'Z'
            timer = new TimeCalculator();
            startingTime = 0;
            isSliding = false;
        }

        public void Execute()
        {
            ObjectManager.Instance.Players[0].EndCrouch();
        }
    }
}

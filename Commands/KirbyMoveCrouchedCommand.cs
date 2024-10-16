using Microsoft.Xna.Framework.Input;
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
            // Accessing the player and keyboard controller through Game1.Instance
            kirby = Game1.Instance.players[0];
            keyboard = Game1.Instance.KeyboardController;
            crouchKey = Keys.Down; // You can set this to whatever key you want for crouching
            attackKey = Keys.Z;    // Similarly, you can set the attack key here
        }

        public void Execute()
        {
            kirby.Crouch();
            if (keyboard.currentState.Contains(attackKey))
            {
                keyboard.stopKeys[crouchKey].Execute();
            }
        }

    }
}

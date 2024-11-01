using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Time;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyEndCrouchCommand : ICommand
    {
        private IPlayer kirby;

        public KirbyEndCrouchCommand()
        {
            // Accessing the player and keyboard controller through Game1.Instance
            kirby = ObjectManager.Instance.Players[0]; // Assuming there is always at least one player
        }

        public void Execute()
        {
            ObjectManager.Instance.Players[0].EndCrouch();
        }
    }
}

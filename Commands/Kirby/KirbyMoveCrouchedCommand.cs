using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveCrouchedCommand : ICommand
    {
        private IPlayer kirby;

        // Constructor with no parameters
        public KirbyMoveCrouchedCommand()
        {
            kirby = ObjectManager.Instance.Players[0];
        }

        public void Execute()
        {
            ObjectManager.Instance.Players[0].Crouch();
        }

    }
}

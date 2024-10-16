using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveRightCommand : ICommand
    {
        public void Execute()
        {
            kirby.MoveRight();

            // Calls corresponding stop key to deal with running/stopping mechanic
            keyboard.stopKeys[key].Execute();
        }
    }
}

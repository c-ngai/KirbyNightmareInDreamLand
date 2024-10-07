using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveRightCommand : ICommand
    {
        private KeyboardController keyboard;
        private Keys key;
        private IPlayer kirby;

        public KirbyMoveRightCommand(IPlayer player, Keys keyMapped, KeyboardController currentKeyboard)
        {
            keyboard = currentKeyboard;
            key = keyMapped;
            kirby = player;
        }

        public void Execute()
        {
            kirby.MoveRight();

            // Calls corresponding stop key to deal with running/stopping mechanic
            keyboard.stopKeys[key].Execute();
        }
    }
}

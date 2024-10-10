using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveLeftCommand : ICommand
    {
        private KeyboardController keyboard;
        private Keys key;
        private IPlayer kirby;

        public KirbyMoveLeftCommand(IPlayer player, Keys keyMapped, KeyboardController currentKeyboard)
        {
            keyboard = currentKeyboard;
            key = keyMapped;
            kirby = player;
        }

        public void Execute()
        {
            kirby.MoveLeft();

            // Calls corresponding stop key to deal with running/stopping mechanic
            keyboard.stopKeys[key].Execute();
        }
    }
}
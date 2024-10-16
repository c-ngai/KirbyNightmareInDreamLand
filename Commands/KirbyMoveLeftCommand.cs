using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveLeftCommand : ICommand
    {
        public void Execute()
        {
            Game1.Instance.players[0].MoveLeft();

            var keyboard = Game1.Instance.KeyboardController;

            if (keyboard.stopKeys.ContainsKey(Keys.Left))
            {
                keyboard.stopKeys[Keys.Left].Execute();
            }
        }
    }
}
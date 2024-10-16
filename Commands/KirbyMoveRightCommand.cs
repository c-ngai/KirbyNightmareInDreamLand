using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Controllers;

namespace KirbyNightmareInDreamLand.Commands
{
    public class KirbyMoveRightCommand : ICommand
    {
        public void Execute()
        {
            Game1.Instance.players[0].MoveRight();

            var keyboard = Game1.Instance.KeyboardController;

            if (keyboard.stopKeys.ContainsKey(Keys.Right))
            {
                keyboard.stopKeys[Keys.Right].Execute();
            }
        }
    }
}
